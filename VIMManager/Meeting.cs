using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMManager 
{ 
    public class Meeting
    {
        public int Id = 1;
        public List<string>? Names { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public override string ToString()
        {
            return $"Number of attendees: {Names!.Count}, Responsible person: {ResponsiblePerson}, " +
                    $"Description: {Description}, Category: {Category}, Type: {Type}, Start: {StartDate:yyyy-MM-dd HH:mm}, " +
                    $"End: {EndDate:yyyy-MM-dd HH:mm}";
        }
        public Meeting(int id, List<string> names, string responsiblePerson, string description, string category, string type, DateTime startdate, DateTime enddate)
        {
            Id = id;
            Names = names;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = category;
            Type = type;
            StartDate = startdate;
            EndDate = enddate;
        }
        public Meeting()
        {

        }
    }
}
