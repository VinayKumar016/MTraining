using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercis
{
    class Program
    {
        static TaskManager taskManager = new TaskManager();

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n==== Task Manager ====");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Create Project");
                Console.WriteLine("3. Assign Project Manager");
                Console.WriteLine("4. Add Developer/QA to Project");
                Console.WriteLine("5. Create Task");
                Console.WriteLine("6. Assign Task to Developer");
                Console.WriteLine("7. Update Task Status");
                Console.WriteLine("8. View All Tasks");
                Console.WriteLine("9. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": taskManager.CreateUser(); break;
                    case "2": taskManager.CreateProject(); break;
                    case "3": taskManager.AssignProjectManager(); break;
                    case "4": taskManager.AssignUserToProject(); break;
                    case "5": taskManager.CreateTask(); break;
                    case "6": taskManager.AssignTask(); break;
                    case "7": taskManager.UpdateTaskStatus(); break;
                    case "8": taskManager.ViewAllTasks(); break;
                    case "9": return;
                    default: Console.WriteLine("Invalid choice! Try again."); break;
                }
            }
        }
    }
}
