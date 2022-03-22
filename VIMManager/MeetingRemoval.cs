using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIMManager
{
    internal class MeetingRemoval : IMeetingListModification
    {
        public bool Execute()
        {            
            //return Data.meetingList.Remove(m.Id);
            return false;
        }
    }
}
