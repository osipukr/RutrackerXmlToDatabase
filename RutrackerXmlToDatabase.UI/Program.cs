using RutrackerXmlToDatabase.Core.Importers;

namespace RutrackerXmlToDatabase.UI
{
    public class Program
    {
        private static string ResoucesPath => @"..\..\..\..\RutrackerXmlToDatabase.Core\Resources\";
        private static string ConntectionString => @"Server=localhost;Database=rutrackers-db;Trusted_Connection=True;";

        private static void Main(string[] args)
        {
            var fileName = "rutracker-20190323.xml.gz";

            TorrentImporter.Import(ConntectionString, ResoucesPath + fileName);
        }
    }
}