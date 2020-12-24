using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.SignalR;
using SharedClass;

namespace SignalR.Hubs
{
    
    public class ChatHub : Hub
    {
        private PrimitiveHolder pH;
        private UserCounter UserCounter; /* consider to read this from file */
        
        public ChatHub(PrimitiveHolder pH, UserCounter userCounter)
        {
            this.pH = pH;
            this.UserCounter = userCounter;

        }

        #region CREATION 
        //method to send a message to all the clients 
        public async Task AddPrimitive(Primitive p)
        {
            p.Id = -1; /* initialize to default */
            pH.Add(p.Name,p); /* how can he modifify this if he is using readonly */
            await Clients.All.SendAsync("OnAddPrimitive", p);
        }

        /*function called from user to have all the primitives created until now*/
        public async Task GetPrimitives()
        {

            PrimitiveHolder primitiveholder = pH;
            await 
                Clients.Caller.SendAsync("RecivePrimitives", primitiveholder, UserCounter.LevelName);
        }


        
        /// <summary>
        /// Load a particular Scene Preset
        /// </summary>
        /// <param name="path">The filename</param>
        /// <returns></returns>
        public async Task LoadScene(string path)
        {
           
            UserCounter.LevelName = path.Replace(".txt", "");
            
            SceneManagerSC SM = new SceneManagerSC();
            PrimitiveHolder NewPH = new PrimitiveHolder();
            List<Primitive> list = SM.Load(path);
            List<String> active = UserCounter.GetActives();
            Primitive o;
            /*Takes all the references to all the active users*/
            foreach (string s in active)
            {
                o = pH.Get(s);
                if(o != null)
                    NewPH.Add(s, o);
            }
            pH.ToBin();
            foreach(Primitive p in list)
            {
                pH.Add(p.Name, p);
            }
            /*send the new set of primitives*/
            pH.ConcatPrimitives(NewPH);
            Console.WriteLine("[Loading] The level loaded is " + UserCounter.LevelName);
            await Clients.All.SendAsync("RecivePrimitives", pH, UserCounter.LevelName);
        }
        #endregion CREATION


        #region UPDATE
        /* Update the local database and
         * send the information of the moved object */
        public async Task ChangePosition(Primitive p) 
        {
            /*UPDATE THE POSITION OF THE ELEMENT*/
         
                 pH.UpdatePosition(p);
            try
            {
                await Clients.Others.SendAsync("OnChangePosition", p);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async Task Collision(Primitive p)
        {
            await Clients.Others.SendAsync("OnCollision", p);
        }

        public async Task ChangeOwnerDirectly(string name, int id)
        {
            /* don 't handle the result */
            pH.ChangeOwner(name, id);
            Primitive p = pH.Get(name);
            await Clients.Others.SendAsync("OnOwnerChange", p);

        }
#endregion

        /*Sets the sessionId and the userId for the caller
         and write the log of who is playing*/
        public async Task<string> GetInfoDirectlyAsync(Primitive p, long UnitySessionId)
        {
            List<Task> tasks = new List<Task>();
            int ID = UserCounter.askForId(Context.ConnectionId);
            string name = ID.ToString();
            p.Id = ID;
            p.Name = name;
            p.Scale = false;
            p.Color = UserCounter.getColor();
            pH.Add(name, p);
            UserCounter.SetSessionId(UnitySessionId);
            string id = ID.ToString();
            long sessionId = UserCounter.SessionID;
            string settings = id + "," + sessionId;
            object record = new { Id = id, SessionId = sessionId, Device = p.Shape };

           
            tasks.Add(Clients.Others.SendAsync("OnAddPrimitive", p));
            /* Task.Run makes a synchronous task asynch*/
            tasks.Add(Task.Run(() =>LogManager.WriteRecord(record, "sessions")));
            await Task.WhenAll(tasks).ConfigureAwait(false);
            /*write the information on a CSV*/
            
            return settings;
        }

      

  

        #region LOG
        public async Task WriteLog(ObjLog obj, string filename)
        {
            
            await Task.Run(() => LogManager.WriteRecord(obj, filename));
        }

        public async Task RecordDataAsync(RecordInfo recordInfo)
        {

            List<Task> tasks = new List<Task>();

            recordInfo.UserCount = UserCounter.GetNActive();
            recordInfo.GUID = UserCounter.SessionID;


            string FileName = "database";

           
            try
            {
                tasks.Add(Task.Run(() => LogManager.WriteRecord(recordInfo, FileName)));
                tasks.Add(Clients.Others.SendAsync("OnSendScore", recordInfo));
                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
           
        }

        #endregion LOG
        //public string GetGetLNameDirectly()
        //{

        //    return userCounter.LevelName;
        //}


        #region DISCONNECTION
        public async Task GoodByeAsync(string name)
        {
            pH.Remove(name);
            UserCounter.removeUser(Context.ConnectionId);
            /* change something in the counter: to be implemented */
            UserCounter.ResetSessionId();
            await SayGoodByeAsync(name);
        }

        /*when someone disconnects, remove it from the users 
         * if she didn't push the button quit*/
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string contextId = Context.ConnectionId;
            string name = UserCounter.getId(contextId);
           /*the user may have already disconnected correctly*/
            if (name != null)
            {
                UserCounter.removeUser(contextId);
                UserCounter.ResetSessionId();
                SayGoodByeAsync(name).Wait();
            }
            return base.OnDisconnectedAsync(exception);
        }

        private async Task SayGoodByeAsync(string name)
        {
            await Clients.Others.SendAsync("OnGoodByeAsync", name);
        }
    }
    #endregion DISCONNECTION

}
