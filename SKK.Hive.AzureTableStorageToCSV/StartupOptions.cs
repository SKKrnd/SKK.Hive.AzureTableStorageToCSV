using System;
using CommandLine;

namespace SKK.Hive.AzureTableStorageToCSV
{
    public class StartupOptions
    {
        /// <summary>
        /// Output files location.
        /// </summary>
        [Option('o', "output", Required = true, HelpText = "Output files base path.")]
        public string OutputPath { get; set; }

        /// <summary>
        /// Exported data date.
        /// </summary>
        [Option('d', "date", Required = true, HelpText = "Export data date (last 7 days) e.g. 20/01/2019.")]
        public DateTime Date { get; set; }

    }
}
