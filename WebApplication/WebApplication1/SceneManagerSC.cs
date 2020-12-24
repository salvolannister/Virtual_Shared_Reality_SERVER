using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;


namespace SignalR
{
    public class SceneManagerSC
    {
        // get the path of where the executable file is 
        private String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
      
        private String jsonString;

        public void SaveScene(List<Primitive> primitives,string name)
        {
         /*to be tested */ 
          jsonString = JsonSerializer.Serialize(primitives);
            System.IO.File.WriteAllText(path+name, jsonString);
        }

        public List<Primitive> Load(string path)
        {
            string fullPath = this.path + "\\" +path;
            jsonString = File.ReadAllText(fullPath, Encoding.UTF8);
            List<Primitive> primitives = JsonSerializer.Deserialize<List<Primitive>>(jsonString);
            return primitives;
        }
    }
}
