namespace RutrackerXmlToDatabase.Core.Models
{
    public class File : BaseModel
    {
        public long Size { get; set; }
        public string Name { get; set; }

        public long TorrentId { get; set; }
        public Torrent Torrent { get; set; }
    }
}