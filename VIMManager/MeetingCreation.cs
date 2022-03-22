using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMManager
{
    public class MeetingCreation : IMeetingListModification
    {
        Meeting m;
        public MeetingCreation()
        {
            m = new();
        }
        public bool Execute()
        {
            
            //if (!Data.meetingList.ContainsKey())
            //{
            //    Data.meetingList.Add(Id,m);
            //    return true;
            //}            
            return false;
        }
    }
}
