
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SharedClass
{
    public class RecordInfo
    {
        public long GUID { get; set; }

        public string Score { get; set; }
        public int Time { get; set; }
        public int UserCount { get; set; }
        public string LevelName { get; set; }


        public RecordInfo(long gUID, string score, int time, int userCount, string levelName)
        {
            GUID = gUID;
            Score = score;
            Time = time;
            UserCount = userCount;
            LevelName = levelName;
        }

        //public void WriteCSV(int userNum, long sessionID)
        //{
        //    string path = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        //    this.UserCount = userNum;
        //    this.GUID = sessionID;
        //    string FileName = path + "\\database.csv";
        //    /* Doesnt rewrite the header if present*/
        //    CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        //    {

        //        HasHeaderRecord = !File.Exists(FileName)
        //    };

        //    var record = new List<RecordInfo> { this };
        //    /*write on a CSV the information*/
        //    using (var fileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
        //    using (var writer = new StreamWriter(fileStream))
        //    using (var csv = new CsvWriter(writer, csvConfig))
        //    {

        //        csv.WriteRecords(record);

        //    }
        //}

       
    }
}