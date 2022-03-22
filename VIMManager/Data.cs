using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace VIMManager
{
    public static class Data
    {
        public static Dictionary<int, Meeting> meetingList = new();
        private static readonly string _path = @"..\..\..\..\Meetings.json";
        public static string? User;
        public static void ReadFile()
        {
            if (File.Exists(_path))
            {
                string jsonString = File.ReadAllText(_path);
                meetingList = JsonSerializer.Deserialize<Dictionary<int, Meeting>>(jsonString)!;
                Meeting.Id = meetingList.Max(b => b.Key) + 1;
            }
            else
            {
                Console.WriteLine("There are no meetings");
            }
        }
        public static void WriteToFile()
        {
            //string path = @"..\..\Meetings.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(meetingList, options);
            File.WriteAllText(_path, jsonString);
        }

    }
}
