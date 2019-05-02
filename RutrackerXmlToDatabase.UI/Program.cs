using RutrackerXmlToDatabase.Core.Importers;
using System.IO;

namespace RutrackerXmlToDatabase.UI
{
    public class Program
    {
        private static string ResoucesPath => @"..\..\..\..\RutrackerXmlToDatabase.Core\Resources";
        private static string ConntectionString => @"Server=localhost;Database=rutrackers-db;Trusted_Connection=True;";

        private static void Main(string[] args)
        {
            var fileName = "rutracker-20190323.xml.gz";
            var path = GetPathToFile(fileName);
            var maxCount = 5000;

            TorrentImporter.Import(ConntectionString, path, maxCount);
        }

        private static string GetPathToFile(string fileName) =>
            Path.Combine(Path.GetFullPath(ResoucesPath), fileName);
    }
}