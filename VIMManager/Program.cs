using System.Globalization;
using VIMManager;

string User = "";
string Command;
Data.ReadFile();
Console.WriteLine("User name:");
while(User.Length < 3)
{
    User = Console.ReadLine()!;
    Data.User = User;
}

while (true)
{
    Console.WriteLine("Avaible commands: [1]-CreateNew, [2]-DeleteMeeting, [3]-AddPerson, [4]-RemovePerson, [5]-ListBy, [6]-Exit.");
    Command = Console.ReadLine()!.ToLower().Trim();
    if (Command == "exit" || Command == "6")
    {
        break;
    }
    ExecutingCommand(Command);
}

static void ExecutingCommand(string command)
{
    switch (command)
    {
        case "createnew" or "1":
            Commands.CreateNew();
            break;
        case "deletemeeting" or "2":
            Commands.DeleteMeetingById();
            break;
        case "addperson" or "3":
            Commands.AddPerson();
            break;
        case "removeperson" or "4":
            Commands.RemovePerson();
            break;
        case "listby" or "5":
            Commands.ListBy();
            break;
        default:
            Console.WriteLine("Wrong command.");
            break;
    }
}