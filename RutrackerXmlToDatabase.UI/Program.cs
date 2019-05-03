using RutrackerXmlToDatabase.Core.Importers;

namespace RutrackerXmlToDatabase.UI
{
    public class Program
    {
        private static string ResourcesPath => @"..\..\..\..\RutrackerXmlToDatabase.Core\Resources\";
        private static string ConnectionString => @"Server=localhost;Database=rutracker-db;Trusted_Connection=True;";

        private static void Main()
        {
            const string fileName = "rutracker-20190323.xml.gz";

            TorrentImporter.Import(ConnectionString, ResourcesPath + fileName);
        }
    }
}