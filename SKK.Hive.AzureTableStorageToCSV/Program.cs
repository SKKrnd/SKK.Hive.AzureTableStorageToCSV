using CommandLine;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Linq;

namespace SKK.Hive.AzureTableStorageToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "<connectionString>";
           
            if (connectionString == "<connectionString>")
            {
                Console.WriteLine($"Please replace \"<connectionString>\" with your connection string.");
                Console.ReadLine();
                //
                return;
            }

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

            var date = new DateTime();
            string path = null;
            //
            Parser.Default.ParseArguments<StartupOptions>(args).WithParsed(o =>
            {
                path = o.OutputPath;
                date = o.Date;
            });
            //
            var maxDate = DateTime.Now.Date.AddDays(-7);
            if (date < maxDate)
            {
                Console.WriteLine($"Date \"{date}\" out of range (\"{maxDate}\").");
                Console.ReadLine();
                //
                return;
            }
            //
            var storageTableReader = new StorageTableReader(cloudStorageAccount);
            //
            Console.WriteLine("Getting tables.");
            var tables = storageTableReader.GetTables().Where(e => Enum.IsDefined(typeof(Tables), e.Name) == true);
            //
            foreach (var table in tables)
            {
                Console.WriteLine($"Getting data for table : \"{table.Name}\".");
                var data = storageTableReader.GetTableData(table.Name, date);
                //
                Console.WriteLine($"Saving {data.Count} to directory : \"{path}\".");
                CSVCreator.TableToCSV(path, data, date, table.Name);
            }
            //
            Console.WriteLine("Press key to exit.");
            Console.ReadLine();
        }
    }
}
