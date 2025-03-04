using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercis
{
    public class TaskManager
    {
        private List<User> users = new List<User>();
        private List<Project> projects = new List<Project>();

        private const string UsersFilePath = "C:\\Users\\Administrator\\Desktop\\users.csv";
        private const string ProjectsFilePath = "C:\\Users\\Administrator\\Desktop\\projects.csv";
        private const string TasksFilePath = "C:\\Users\\Administrator\\Desktop\\tasks.csv";

        public TaskManager()
        {
            LoadData();
        }

        public void CreateUser()
        {
            Console.Write("Enter User Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Role (Admin, ProjectManager, Developer, QA): ");
            if (Enum.TryParse(Console.ReadLine(), out UserRole role))
            {
                users.Add(new User(name, role));
                Console.WriteLine("User Created Successfully!");
                SaveData();
            }
            else
            {
                Console.WriteLine("Invalid Role!");
            }
        }

        public void CreateProject()
        {
            Console.Write("Enter Project Name: ");
            string projectName = Console.ReadLine();
            projects.Add(new Project(projectName));
            Console.WriteLine("Project Created Successfully!");
            SaveData();
        }

        public void AssignProjectManager()
        {
            Console.Write("Enter Project Name: ");
            string projectName = Console.ReadLine();
            Project project = projects.Find(p => p.ProjectName == projectName);
            if (project == null)
            {
                Console.WriteLine("Project Not Found!");
                return;
            }

            Console.Write("Enter Manager Name: ");
            string managerName = Console.ReadLine();
            User manager = users.Find(u => u.Name == managerName && u.Role == UserRole.ProjectManager);
            if (manager == null)
            {
                Console.WriteLine("Manager Not Found!");
                return;
            }

            project.ProjectManager = manager;
            Console.WriteLine("Project Manager Assigned Successfully!");
            SaveData();
        }

        public void AssignUserToProject()
        {
            Console.Write("Enter Project Name: ");
            string projectName = Console.ReadLine();
            Project project = projects.Find(p => p.ProjectName == projectName);
            if (project == null)
            {
                Console.WriteLine("Project Not Found!");
                return;
            }

            Console.Write("Enter User Name: ");
            string userName = Console.ReadLine();
            User user = users.Find(u => u.Name == userName);
            if (user == null)
            {
                Console.WriteLine("User Not Found!");
                return;
            }

            if (user.Role == UserRole.Developer)
                project.Developers.Add(user);
            else if (user.Role == UserRole.QA)
                project.QAs.Add(user);

            Console.WriteLine("User Assigned to Project!");
            SaveData();
        }

        public void CreateTask()
        {
            Console.Write("Enter Project Name: ");
            string projectName = Console.ReadLine();
            Project project = projects.Find(p => p.ProjectName == projectName);
            if (project == null)
            {
                Console.WriteLine("Project Not Found!");
                return;
            }

            Console.Write("Enter Task Name: ");
            string taskName = Console.ReadLine();
            Console.Write("Enter Task Description: ");
            string description = Console.ReadLine();

            TaskItem task = new TaskItem(taskName, description);
            project.Tasks.Add(task);
            Console.WriteLine("Task Created Successfully!");
            SaveData();
        }

        public void AssignTask()
        {
            Console.Write("Enter Project Name: ");
            string projectName = Console.ReadLine();
            Project project = projects.Find(p => p.ProjectName == projectName);

            if (project == null)
            {
                Console.WriteLine("Project Not Found!");
                return;
            }

            var unassignedTasks = project.Tasks.Where(t => t.AssignedTo == null).ToList();
            if (unassignedTasks.Count == 0)
            {
                Console.WriteLine("No unassigned tasks available.");
                return;
            }

            Console.WriteLine("\nAvailable Tasks:");
            for (int i = 0; i < unassignedTasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {unassignedTasks[i].TaskName}");
            }

            Console.Write("Select a Task Number: ");
            if (!int.TryParse(Console.ReadLine(), out int taskIndex) || taskIndex < 1 || taskIndex > unassignedTasks.Count)
            {
                Console.WriteLine("Invalid task selection!");
                return;
            }

            TaskItem selectedTask = unassignedTasks[taskIndex - 1];

            if (project.Developers.Count == 0)
            {
                Console.WriteLine("No developers available to assign.");
                return;
            }

            Console.WriteLine("\nAvailable Developers:");
            for (int i = 0; i < project.Developers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {project.Developers[i].Name}");
            }

            Console.Write("Select a Developer Number: ");
            if (!int.TryParse(Console.ReadLine(), out int devIndex) || devIndex < 1 || devIndex > project.Developers.Count)
            {
                Console.WriteLine("Invalid developer selection!");
                return;
            }

            User assignedDeveloper = project.Developers[devIndex - 1];

            selectedTask.AssignedTo = assignedDeveloper;
            selectedTask.Status = TaskStatus.Development;

            Console.WriteLine($"Task '{selectedTask.TaskName}' assigned to {assignedDeveloper.Name} successfully!");
            SaveData();
        }

        public void UpdateTaskStatus()
        {
            Console.Write("Enter Project Name: ");
            string projectName = Console.ReadLine();
            Project project = projects.Find(p => p.ProjectName == projectName);

            if (project == null)
            {
                Console.WriteLine("Project Not Found!");
                return;
            }

            Console.WriteLine("\nTasks in Project:");
            for (int i = 0; i < project.Tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {project.Tasks[i].TaskName} - [{project.Tasks[i].Status}]");
            }

            Console.Write("Select a Task Number to Update Status: ");
            if (!int.TryParse(Console.ReadLine(), out int taskIndex) || taskIndex < 1 || taskIndex > project.Tasks.Count)
            {
                Console.WriteLine("Invalid task selection!");
                return;
            }

            TaskItem selectedTask = project.Tasks[taskIndex - 1];

            Console.WriteLine("\nSelect New Status:");
            foreach (TaskStatus status in Enum.GetValues(typeof(TaskStatus)))
            {
                Console.WriteLine($"{(int)status}. {status}");
            }

            Console.Write("Enter Status Number: ");
            if (!int.TryParse(Console.ReadLine(), out int statusIndex) || !Enum.IsDefined(typeof(TaskStatus), statusIndex))
            {
                Console.WriteLine("Invalid status selection!");
                return;
            }

            selectedTask.Status = (TaskStatus)statusIndex;
            Console.WriteLine($"Task '{selectedTask.TaskName}' updated to '{selectedTask.Status}' successfully!");
            SaveData();
        }

        public void ViewAllTasks()
        {
            if (projects.Count == 0)
            {
                Console.WriteLine("No projects available!");
                return;
            }

            Console.WriteLine("\nAll Tasks:");
            foreach (var project in projects)
            {
                Console.WriteLine($"\n📌 Project: {project.ProjectName}");

                if (project.Tasks.Count == 0)
                {
                    Console.WriteLine("   No tasks available.");
                    continue;
                }

                foreach (var task in project.Tasks)
                {
                    string assignedTo = task.AssignedTo != null ? task.AssignedTo.Name : "Unassigned";
                    Console.WriteLine($"   🔹 Task: {task.TaskName} | Status: {task.Status} | Assigned To: {assignedTo}");
                }
            }
        }

        private void SaveData()
        {
            SaveUsers();
            SaveProjects();
            SaveTasks();
        }

        private void LoadData()
        {
            LoadUsers();
            LoadProjects();
            LoadTasks();
        }

        private void SaveUsers()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(UsersFilePath))
                {
                    foreach (var user in users)
                    {
                        writer.WriteLine($"{user.Name},{user.Role}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }

        private void LoadUsers()
        {
            if (File.Exists(UsersFilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(UsersFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 2 && Enum.TryParse(parts[1], out UserRole role))
                            {
                                users.Add(new User(parts[0], role));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading users: {ex.Message}");
                }
            }
        }

        private void SaveProjects()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ProjectsFilePath))
                {
                    foreach (var project in projects)
                    {
                        string managerName = project.ProjectManager != null ? project.ProjectManager.Name : "";
                        writer.WriteLine($"{project.ProjectName},{managerName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving projects: {ex.Message}");
            }
        }

        private void LoadProjects()
        {
            if (File.Exists(ProjectsFilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(ProjectsFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 2)
                            {
                                var project = new Project(parts[0]);
                                if (!string.IsNullOrEmpty(parts[1]))
                                {
                                    project.ProjectManager = users.Find(u => u.Name == parts[1]);
                                }
                                projects.Add(project);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading projects: {ex.Message}");
                }
            }
        }

        private void SaveTasks()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(TasksFilePath))
                {
                    foreach (var project in projects)
                    {
                        foreach (var task in project.Tasks)
                        {
                            string assignedTo = task.AssignedTo != null ? task.AssignedTo.Name : "";
                            writer.WriteLine($"{project.ProjectName},{task.TaskName},{task.Description},{assignedTo},{task.Status}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }

        private void LoadTasks()
        {
            if (File.Exists(TasksFilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(TasksFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var parts = line.Split(',');
                            if (parts.Length == 5 && Enum.TryParse(parts[4], out TaskStatus status))
                            {
                                var project = projects.Find(p => p.ProjectName == parts[0]);
                                if (project != null)
                                {
                                    var task = new TaskItem(parts[1], parts[2])
                                    {
                                        Status = status,
                                        AssignedTo = users.Find(u => u.Name == parts[3])
                                    };
                                    project.Tasks.Add(task);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading tasks: {ex.Message}");
                }
            }
        }
    }

    public class User
    {
        public string Name { get; set; }
        public UserRole Role { get; set; }

        public User(string name, UserRole role)
        {
            Name = name;
            Role = role;
        }
    }

    public class Project
    {
        public string ProjectName { get; set; }
        public User ProjectManager { get; set; }
        public List<User> Developers { get; set; } = new List<User>();
        public List<User> QAs { get; set; } = new List<User>();
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public Project(string projectName)
        {
            ProjectName = projectName;
        }
    }

    public class TaskItem
    {
        public string TaskName { get; set; }
        public string Description { get; set; }
        public User AssignedTo { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.New;

        public TaskItem(string taskName, string description)
        {
            TaskName = taskName;
            Description = description;
        }
    }

    public enum UserRole
    {
        Admin,
        ProjectManager,
        Developer,
        QA
    }

    public enum TaskStatus
    {
        New,
        Development,
        Testing,
        Completed
    }
}
