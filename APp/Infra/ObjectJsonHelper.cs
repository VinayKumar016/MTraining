using System.Text.Json;
namespace FoodDeliveryMVC.Infra
{
    public class ObjectJsonHelper
    {
        public static string ToJson<T>(T obj)
        {
            return JsonSerializer.Serialize<T>(obj);
        }

        public static T GetFromJson<T>(string strInp)
        {
            return JsonSerializer.Deserialize<T>(strInp);
        }
    }
}
