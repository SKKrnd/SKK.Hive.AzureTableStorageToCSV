using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace SKK.Hive.AzureTableStorageToCSV
{
    public static class CSVCreator
    {
        /// <summary>
        /// Create CSV file with data.
        /// </summary>
        /// <param name="basePath"> Output files location.</param>
        /// <param name="data">Table data.</param>
        /// <param name="date">Export data date.</param>
        /// <param name="tableName">Table name.</param>
        public static void TableToCSV(string basePath, List<Dictionary<string, string>> data, DateTime date, string tableName)
        { 
            string path = $"{basePath}\\{date.Year.ToString()}\\{date.Month.ToString()}\\{date.Day.ToString()}";
            //
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //
            var keys = new List<string>();
            //
            foreach (var item in data)
            {
                foreach(var property in item)
                {
                    if (!keys.Contains(property.Key))
                        keys.Add(property.Key);
                }
            }
            //
            string header = String.Join(";", keys.Select(x => x.ToString()).ToArray());
            //
            using (var sw = File.CreateText(Path.Combine(path, $"{date.Date.ToString("yyyy.MM.dd")}_{tableName}.csv")))
            {
                sw.WriteLine(header);
               //
                foreach (var item in data)
                {
                    var sb = new StringBuilder();
                    string line = "";
                    //
                    foreach (var key in keys)
                    {
                        line = item.ContainsKey(key) == true ? line + item[key] + "; " : string.Empty + "; ";
                    }
                    //
                    sb.AppendLine(line.Substring(0, line.Length - 2));
                    sw.Write(sb.ToString());
                }
            }
        }
    }
}
