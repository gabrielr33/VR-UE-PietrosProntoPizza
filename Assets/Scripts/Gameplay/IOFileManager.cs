using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gameplay
{
    public static class IOFileManager
    {
        private static string _customerNamesFilePath = "";

        public static List<string> ReadCustomerNamesMales()
        {
            string[] customerNames = ReadAllCustomerNames();
            if (customerNames.Length == 0)
                return new List<string>();

            string[] maleNames = customerNames[0].Split(",");
            
            return ReplaceSpecialCharacters(maleNames);
        }

        public static List<string> ReadCustomerNamesFemales()
        {
            string[] customerNames = ReadAllCustomerNames();
            if (customerNames.Length == 0)
                return new List<string>();

            string[] femaleNames = customerNames[1].Split(",");

            return ReplaceSpecialCharacters(femaleNames);
        }

        private static string[] ReadAllCustomerNames()
        {
            try
            {
                _customerNamesFilePath =
                    $"{Directory.GetCurrentDirectory()}\\Assets\\Resources\\GameData\\CustomerNames.txt";
                string data = File.ReadAllText(_customerNamesFilePath);
                return data.Split(";");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unable to read from customer name file: {ex.Message}");
                return Array.Empty<string>();
            }
        }

        private static List<string> ReplaceSpecialCharacters(string[] names)
        {
            List<string> escapedNames = new List<string>();
            foreach (string name in names)
                escapedNames.Add(name.Replace("\r", "").Replace("\n", ""));

            return escapedNames;
        }
    }
}