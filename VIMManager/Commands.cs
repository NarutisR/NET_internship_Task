using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMManager
{
    public static class Commands
    {
        public static void CreateNew()
        {
            Meeting meeting = new();
            meeting.Names = new List<string>();
            meeting.Names.AddRange(ReadNames());
            MakeResponsible(meeting.Names, meeting);
            Console.WriteLine("Write description of this meeting");
            while ((meeting.Description = Console.ReadLine()) == "");
            ChooseCategory(meeting);
            ChooseType(meeting);
            SetStartDate(meeting);
            SetEndDate(meeting);
            Data.meetingList.Add(Meeting.Id++, meeting);
            Data.WriteToFile();
        }
        private static List<string> ReadNames()
        {
            HashSet<string> nm = new();
            Console.WriteLine("Leave line empty to stop listing attendees.");
            string? temp = " ";
            while (temp != "")
            {
                temp = Console.ReadLine()!;
                if (temp != "")
                {
                    if (!nm.Add(temp))
                    {
                        Console.WriteLine("Person with this name already exists.");
                    }
                }
            }
            return nm.ToList();
        }
        private static bool CheckIfPersonExists(string? name, List<string> names)
        {
            bool exists = false;
            if (names == null)
            {
                return exists;
            }
            foreach (string n in names)
            {
                if (n.ToLower() == name!.ToLower())
                {
                    exists = true;
                }
            }
            return exists;
        }
        private static void MakeResponsible(List<string> names, Meeting m)
        {
            while (true)
            {  
                if (CheckIfPersonExists(Data.User, names))
                {
                    break;
                }
                else
                {
                    m.Names!.Add(Data.User!);
                }
            }
            m.ResponsiblePerson = Data.User;
        }
        private static void ChooseCategory(Meeting m)
        {
            string temp;
            Console.WriteLine("Choose one of the category: [1]-CodeMonkey, [2]-Hub, [3]-Short, [4]-TeamBuilding");
            while (true)
            {
                temp = Console.ReadLine()!.ToLower();
                switch (temp)
                {
                    case "codemonkey" or "1":
                        m.Category = "CodeMonkey";
                        return;
                    case "hub" or "2":
                        m.Category = "Hub";
                        return;
                    case "short" or "3":
                        m.Category = "Short";
                        return;
                    case "teambuilding" or "4":
                        m.Category = "TeamBuilding";
                        return;
                   default:
                        break;
                }
            }
        }
        private static void ChooseType(Meeting m)
        {
            string temp;
            Console.WriteLine("Choose one of the type: [1]-Live, [2]-InPerson");
            while (true)
            {
                temp = Console.ReadLine()!.ToLower();
                switch (temp)
                {
                    case "live" or "1":
                        m.Type = "Live";
                        return;
                    case "inperson" or "2":
                        m.Type = "InPerson";
                        return;
                    default:
                        break;
                }
            }
        }

        private static void SetStartDate(Meeting m)
        {
            bool time = false;
            DateTime start;
            Console.WriteLine("Write start date of the meeting.");
            while (!time)
            {
                while (!DateTime.TryParse(Console.ReadLine(), out start)) ;
                if (start > DateTime.Now)
                {
                    m.StartDate = start;
                    time = true;
                }
                else
                {
                    Console.WriteLine("Try again.");
                }
            }
        }
        private static void SetEndDate(Meeting m)
        {
            bool time = false;
            DateTime end;
            Console.WriteLine("Write end date of the meeting.");
            while (!time)
            {                
                while (!DateTime.TryParse(Console.ReadLine(), out end)) ;
                if (end > m.StartDate)
                {
                    m.EndDate = end;
                    time = true;
                }
                else
                {
                    Console.WriteLine("Try again");
                }
            }
        }
        public static void DeleteMeetingById()
        {
            if (Data.meetingList.Count == 0)
            {
                Console.WriteLine("There are no meetings.");
                return;
            }
            string input;
            Console.WriteLine("Write id of the meeting to delete.");
            while (true)
            {
                input = Console.ReadLine()!;
                _ = int.TryParse(input, out int id);
                if (Data.meetingList.ContainsKey(id) && Data.User == Data.meetingList[id].ResponsiblePerson)
                {
                    Data.meetingList.Remove(id);
                    Data.WriteToFile();
                    Console.WriteLine("Meeting was deleted.");
                    return;
                }
                else if (input == "")
                {
                    return;
                }
                else
                {
                    Console.WriteLine($"{Data.User} is not responsible for this meeting.");
                }
            }
        }
        public static void AddPerson()
        {
            if (Data.meetingList.Count == 0)
            {
                Console.WriteLine("There are no meetings.");
                return;
            }
            int Id;
            string name;
            Console.WriteLine("Write Id of the meeting to add person:");
            while (!int.TryParse(Console.ReadLine(), out Id)) ;
            Console.WriteLine("Write person's name to add:");
            if (Data.meetingList[Id].Names == null)
            {
                Data.meetingList[Id].Names = new List<string>();
            }
            while (true)
            {
                name = Console.ReadLine()!;
                if (CheckIfPersonExists(name, Data.meetingList[Id].Names!))
                {
                    Console.WriteLine("Person already in meeting.");
                }
                else
                {
                    if (name != "")
                    {
                        WarnIfDateTimeInterects(name, Id);
                        Data.meetingList[Id].Names!.Add(name);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Data.WriteToFile();
        }
        private static void WarnIfDateTimeInterects(string name, int id)
        {
            if (Data.meetingList[id].Names == null)
            {
                return;
            }
            var tempList = Data.meetingList.Where(l => l.Value.Names!.Contains(name)).ToDictionary(k => k.Key, v => v.Value);
            foreach (var pair in tempList)
            {
                if ((pair.Value.StartDate < Data.meetingList[id].EndDate && pair.Value.EndDate > Data.meetingList[id].StartDate))
                {
                    Console.WriteLine($"Person {name} already in the  meeting(Id:{pair.Key}) at this time.");
                }
            }
        }
        public static void RemovePerson()
        {
            string tempName;
            string input;
            int Id;
            if (Data.meetingList.Count == 0)
            {
                Console.WriteLine("There are no meetings");
                return;
            }
            Console.WriteLine("Write id of the meeting.");
            while (true)
            {
                input = Console.ReadLine()!;
                if (input == "")
                {
                    return;
                }
                if(int.TryParse(input, out Id) && Data.meetingList.ContainsKey(Id))
                {
                    break;
                }
            }
            Console.WriteLine("Write person's name to remove.");
            while(true)
            {
                tempName = Console.ReadLine()!;
                if(tempName == "")
                {
                    return;
                }
                if (Data.meetingList[Id].Names!.Contains(tempName) && Data.meetingList[Id].ResponsiblePerson != tempName)
                {
                    Data.meetingList[Id].Names!.Remove(tempName);
                    Console.WriteLine($"{tempName} was succesfully removed from the meeting.");
                    return;
                }
            }
        }
        public static void ListBy()
        {
            if (Data.meetingList.Count == 0)
            {
                Console.WriteLine("There are no meetings.");
                return;
            }
            string command;
            Console.WriteLine("List by: [1]-All, [2]-Description, [3]-ResponsiblePerson, [4]-Category, [5]-Type, [6]-Date, [7]-NumberOfAttendees.");
            while(true)
            {
                command = Console.ReadLine()!.ToLower().Trim();
                if(PrintByOptions(command))
                {
                    return;
                }
            }
        }
        private static bool PrintByOptions(string command)
        {
            switch (command)
            {
                case "all" or "1":
                    PrintAll(Data.meetingList);
                    return true;
                case "description" or "2":
                    PrintAll(FindByDescription());
                    return true;
                case "responsibleperson" or "3":
                    PrintAll(FindByResponsiblePerson());
                    return true;
                case "category" or "4":
                    PrintAll(FindByCategory());
                    return true;
                case "type" or "5":
                    PrintAll(FindByType());
                    return true;
                case "date" or "6":
                    PrintAll(FindByDate());
                    return true;
                case "numberofattendees" or "7":
                    PrintAll(FindByMinNumberOfAttendees());
                    return true;
                default:
                    Console.WriteLine("Wrong command.");
                    return false;
            }
        }
        private static void PrintAll(Dictionary<int, Meeting> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine($"Id: {item.Key}, " + item.Value);
                Console.WriteLine();
            }
        }
        private static Dictionary<int, Meeting> FindByDescription()
        {
            Console.WriteLine("Write fraze to search:");
            string text = "";
            text = Console.ReadLine()!.ToLower().Trim();
            return Data.meetingList.Where(s => s.Value.Description!.ToLower().Contains(text)).ToDictionary(k => k.Key, v => v.Value);
        }
        private static Dictionary<int, Meeting> FindByResponsiblePerson()
        {
            Console.WriteLine("Write responsible person's name:");
            string text = Console.ReadLine()!.ToLower().Trim();
            return Data.meetingList.Where(s => s.Value.ResponsiblePerson!.ToLower().Contains(text)).ToDictionary(k => k.Key, v => v.Value);
        }
        private static Dictionary<int, Meeting> FindByCategory()
        {
            string category = "";
            string input;
            bool right = false;
            Console.WriteLine("Choose category: [1]-CodeMonkey, [2]-Hub, [3]-Short, [4]-TeamBuilding");
            while (!right)
            {
                input = Console.ReadLine()!.ToLower().Trim();
                switch (input)
                {
                    case "codemonkey" or "1":
                        category = "CodeMonkey";
                        right = true;
                        break;
                    case "hub" or "2":
                        category = "Hub";
                        right = true;
                        break;
                    case "short" or "3":
                        category = "Short";
                        right = true;
                        break;
                    case "teambuilding" or "4":
                        category = "TeamBuilding";
                        right = true;
                        break;
                    default:
                        break;
                }
            }
            return Data.meetingList.Where(s => s.Value.Category == category).ToDictionary(k => k.Key, v => v.Value);
        }
        private static Dictionary<int, Meeting> FindByType()
        {
            string type = "";
            string input;
            bool right = false;
            Console.WriteLine("Choose type: [1]-Live, [2]-InPerson");
            while (!right)
            {
                input = Console.ReadLine()!.ToLower().Trim();
                switch (input)
                {
                    case "live" or "1":
                        type = "Live";
                        right = true;
                        break;
                    case "inperson" or "2":
                        type = "InPerson";
                        right = true;
                        break;
                    default:
                        break;
                }
            }
            return Data.meetingList.Where(s => s.Value.Type == type).ToDictionary(k => k.Key, v => v.Value);
        }
        private static Dictionary<int, Meeting> FindByDate()
        {
            Dictionary<int, Meeting> result = new();
            List<DateTime> list = new ();
            string timeString = " ";
            Console.WriteLine("To find meetings which starts from date, write one date and empty line. To find meetings in between input two dates.");
            while (true)
            {
                timeString = Console.ReadLine()!;
                if(DateTime.TryParse(timeString, out DateTime time) || timeString == "")
                {
                    if (list.Count == 0 && timeString == "")
                    {
                        break;
                    }
                    else if (list.Count == 0 && timeString != "")
                    {
                        list.Add(time);
                    }
                    else if (list.Count == 1 && timeString == "")
                    {
                        result = Data.meetingList.Where(s => s.Value.StartDate >= list[0]).ToDictionary(k => k.Key, v => v.Value);
                        break;
                    }
                    else if (list.Count == 1 && timeString != "")
                    {
                        if (time > list[0])
                        {
                            list.Add(time);
                            result = Data.meetingList.Where(s => s.Value.StartDate >= list[0] && s.Value.StartDate <= list[1]).ToDictionary(k => k.Key, v => v.Value);
                            break;
                        }
                    }
                }
            }
            return result;
        }
        private static Dictionary<int, Meeting> FindByMinNumberOfAttendees()
        {
            string str;
            Console.WriteLine("Input min number of attendees:");
            while (true)
            {
                str = Console.ReadLine()!;
                if(int.TryParse(str, out int number))
                {
                    return Data.meetingList.Where(s => s.Value.Names!.Count >= number).ToDictionary(k => k.Key, v => v.Value);
                }
            }
        }
    }
}
