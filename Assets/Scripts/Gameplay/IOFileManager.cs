using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Gameplay
{
    public static class IOFileManager
    {
        // public static GameValues GameValues { get; private set; }
        
        private static string _gameValuesFilePath = "";
        private static GameValues _gameValues;

        public static GameValues ReadGameValuesFromFile()
        {
            _gameValuesFilePath = $"{Directory.GetCurrentDirectory()}\\Assets\\Resources\\GameData\\GameValues.json";
            _gameValues = JsonConvert.DeserializeObject<GameValues>(File.ReadAllText(_gameValuesFilePath));
            return _gameValues;
        }
        
        public static List<string> ReadCustomerNamesMales()
        {
            List<string> maleNames = _gameValues.CustomerNamesMale.Split(",").ToList();

            return maleNames; //ReplaceSpecialCharacters(maleNames);
        }

        public static List<string> ReadCustomerNamesFemales()
        {
            List<string>  femaleNames = _gameValues.CustomerNamesFemale.Split(",").ToList();

            return femaleNames; //ReplaceSpecialCharacters(femaleNames);
        }

        private static List<string> ReplaceSpecialCharacters(string[] names)
        {
            List<string> escapedNames = new List<string>();
            foreach (string name in names)
                escapedNames.Add(name.Replace("\r", "").Replace("\n", ""));

            return escapedNames;
        }
    }

    public class GameValues
    {
        public string CustomerNamesMale { get; set; }
        public string CustomerNamesFemale { get; set; }
        public int CustomerMinWaitTime { get; set; }
        public int CustomerMaxWaitTime { get; set; }
    }
}