using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR
{
    /* class used to share information between this application and 
     * the HMD and Tablet one */ 
    public class Primitive
    {
        

        public string Name { get; set; }

        public string Shape { get; set; }
 
        public float X { get; set; }
 
        public float Y { get; set; }

        public float Z { get; set; }
        public int Id { get; set; }

        public float RotX { get; set; }

        public float RotY { get; set; }

        public float RotZ { get; set; }

        public float W { get; set; }

        public float Sx { get; set; }

        public float Sy { get; set; }

        public float Sz { get; set; }

        public bool Scale { get; set; }
        public string Color { get; set; }
        public bool Avatar { get; set; }
     
    }
}
