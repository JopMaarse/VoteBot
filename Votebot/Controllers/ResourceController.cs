using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Votebot.Controllers
{
    public static class ResourceController
    {
        public static string GetToken()
        {
            const string PATH = @"Resources/token.txt";
            return File.ReadAllText(PATH);
        }

        public static IList<string> GetDefaultOptions()
        {
            const string PATH = @"Resources/default_options.txt";
            return File.ReadAllLines(PATH).ToList();
        }

        public static void SetDefaultOptions(params string[] options)
        {
            const string PATH = @"Resources/default_options.txt";
            File.WriteAllLines(PATH, options);
        }

        public static void SetVoteDelay(int seconds)
        {
            const string PATH = @"Resources/settings.json";
            string text = File.ReadAllText(PATH);
            JObject json = JObject.Parse(text);
            json["Vote"]["delay"] = seconds;
            text = JsonConvert.SerializeObject(json, Formatting.Indented);
            File.WriteAllText(PATH, text);
        }

        public static int GetVoteDelay()
        {
            const string PATH = @"Resources/settings.json";
            string text = File.ReadAllText(PATH);
            JObject json = JObject.Parse(text);
            return (int) json["Vote"]["delay"];
        }

        public static string GetPrefix()
        {
            const string PATH = @"Resources/settings.json";
            string text = File.ReadAllText(PATH);
            JObject json = JObject.Parse(text);
            return json["Prefix"].ToString();
        }

        public static char GetSeparator()
        {
            const string PATH = @"Resources/settings.json";
            string text = File.ReadAllText(PATH);
            JObject json = JObject.Parse(text);
            return json["Separator"].ToString()[0];
        }

        public static bool GetMultipleOptionsAllowed()
        {
            const string PATH = @"Resources/settings.json";
            string text = File.ReadAllText(PATH);
            JObject json = JObject.Parse(text);
            return (bool) json["MultipleOptions"];
        }
    }
}
