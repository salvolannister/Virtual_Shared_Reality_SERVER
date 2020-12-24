using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR
{
    /* Class used to save information about the primitives already generated 
     * and to keep track of their updates*/
    public class PrimitiveHolder
    {
        public Dictionary<string, Primitive> primitives = new Dictionary<string, Primitive>();

        public void UpdatePosition(Primitive primitive)
        {
           
            if( primitives.TryGetValue(primitive.Name, out Primitive result))
            {
                result.RotX = primitive.RotX;
                result.RotY = primitive.RotY;
                result.RotZ = primitive.RotZ;
                result.W = primitive.W;
                result.X = primitive.X;
                result.Y = primitive.Y;
                result.Z = primitive.Z;
                result.Id = primitive.Id;
                if (primitive.Scale == true)
                {
                    result.Sx = primitive.Sx;
                    result.Sy = primitive.Sy;
                    result.Sz = primitive.Sz;
                }
            }
           
            
        }


        public void ConcatPrimitives(PrimitiveHolder B)
        {
            /* shouldnt be done like this :') */
            B.primitives.ToList().ForEach(x => primitives.Add(x.Key, x.Value));
            //primitives.Concat(B.primitives).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        public List<Primitive> getList()
        {
            
            return primitives.Values.ToList();
        }

        public void Add(string name, Primitive p)
        {
            primitives.Add(name, p);
        }

        public Primitive Get(string name)
        {
            Primitive p;
            primitives.TryGetValue(name, out p);
            return p;
        }
        public int Count()
        {
            return primitives.Count();
        }

        public bool ChangeOwner ( string name, int id)
        {
            Primitive p;
            if (primitives.TryGetValue(name, out p))
            {

                if (p.Id == -1)
                {
                    p.Id = id;
                    return true;
                }
                else if (p.Id == id)
                {
                    return true;
                }
                else if (id == -1 && p.Id != -1)
                {
                    p.Id = id;
                    return true;

                }
              
            }
            Console.WriteLine("[ERROR] Trying to access to a gameobject " + name + " that doesn't exist anymore ");
            return false;
            
          
        }

        public void Remove(string name)
        {
            primitives.Remove(name);
        }

        public void ToBin()
        {
            primitives = new Dictionary<string, Primitive>();
        }
    }
}
