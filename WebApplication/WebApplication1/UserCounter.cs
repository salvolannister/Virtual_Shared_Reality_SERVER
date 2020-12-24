using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR
{
    public class UserCounter
    {
        int MAX_ID = 100;
        int current = 0;
        private Dictionary<string,string> active = new Dictionary<string, string>();
        long sessionID = -1;
        private string levelName ="NoLevel";
        public string LevelName { get => levelName; set => levelName = value; }
        public long SessionID { get => sessionID; set => sessionID = value; }

        enum Colors { black,
                      white,
                      blue,
                      cyan,
                      grey,
                      magenta,
                      yellow,
                      red,
                      green
        }
        private void Increment()
        {
            current++;
        }

        public List<String> GetActives()
        {
            return active.Values.ToList();
        }
        public int GetNActive()
        {
            return active.Values.Count;
        }
        public string getColor()
        {
            Colors actual = (Colors)current;
            return actual.ToString();
        }
        public int askForId(string currentID)
        {
           
            if(current >= MAX_ID)
            {
                System.Console.WriteLine("Total number of user reached");
                return -1;
            }
            else
            {
                int retVal = current;
                active.Add(currentID, retVal.ToString());
                Increment();
                return retVal;
            }

        }

        internal bool removeUser(string connectionID)
        {
          return  active.Remove(connectionID);


        }

        public string getId(string name)
        {
            string id = null;
             active.TryGetValue(name, out id );
            return id;
        }

        public void SetSessionId(long sId)
        {
            long current = SessionID;
            if (current == -1)
                 SessionID = sId;
            else
                Console.WriteLine(" Session id was already present!");
        }

        internal void ResetSessionId()
        {
            SessionID = -1;
        }
    }
}
