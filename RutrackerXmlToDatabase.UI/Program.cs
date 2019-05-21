using System.Threading.Tasks;
using RutrackerXmlToDatabase.Core.Importers;

namespace RutrackerXmlToDatabase.UI
{
    public class Program
    {
        private static string ResourcesPath => @"..\..\..\..\RutrackerXmlToDatabase.Core\Resources\";
        private static string ConnectionString => @"Server=localhost;Database=rutracker-db;Trusted_Connection=True;";

        private static async Task Main()
        {
            const string fileName = "rutracker-20190323.xml.gz";

            await TorrentImporter.ImportAsync(ConnectionString, ResourcesPath + fileName);
        }
    }
}