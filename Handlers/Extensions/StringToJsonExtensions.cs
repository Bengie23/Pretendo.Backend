using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Pretendo.Backend.Handlers.Extensions
{
    public static class StringToJsonExtensions
    {
        public static string FromPretendoString(this string value)
        {
            if (value.PassesDefaultJsonChecks())
            {
                return value.Replace("'", "\"");
            }
            return value;
        }
        public static bool IsValidJson(this string strInput)
        {
            if (strInput.PassesDefaultJsonChecks()){
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return false;
        }

        private static bool PassesDefaultJsonChecks(this string jsonInput)
        {
            if (!string.IsNullOrWhiteSpace(jsonInput))
            {
                jsonInput = jsonInput.Trim();
                if ((jsonInput.StartsWith("{") && jsonInput.EndsWith("}")) || //For object
                    (jsonInput.StartsWith("[") && jsonInput.EndsWith("]"))) //For array
                {
                    return true;
                }
            }
            return false;
        }
    }
}
