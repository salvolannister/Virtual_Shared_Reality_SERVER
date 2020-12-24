using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR
{
    public class LogManager
    {


        /* to move in another class */
        public static void WriteRecord(object m_record, string m_fileName)
        {
            string path = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);


            m_fileName = path + "\\" + m_fileName + ".csv";
            /* Doesnt rewrite the header if present*/
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {

                HasHeaderRecord = !File.Exists(m_fileName)
            };



            var record = new List<object> { m_record };
            /*write on a CSV the information*/
            using (var fileStream = new FileStream(m_fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var writer = new StreamWriter(fileStream))
            using (var csv = new CsvWriter(writer, csvConfig))
            {

                csv.WriteRecords(record);

            }

        }
    }


}
