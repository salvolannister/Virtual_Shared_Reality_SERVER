using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharedClass
{
    public class ObjLog
    {
        /* if encapsulated they will not appear as headers in csv!*/
        public int id { get; set; }
        public long sessionId { get; set; }
        public string obj { get; set; }

        public string level { get; set; }
        public int translation { get; set; }
        public int rotation { get; set; }
        public int scale { get; set; }
        public float time { get; set; }

        public ObjLog(string obj, int translation, int rotation, int scale, float time, long sessionId, int id, string level)
        {
            this.obj = obj;
            this.translation = translation;
            this.rotation = rotation;
            this.scale = scale;
            this.time = time;
            this.sessionId = sessionId;
            this.id = id;
            this.level = level;
        }
    }
}