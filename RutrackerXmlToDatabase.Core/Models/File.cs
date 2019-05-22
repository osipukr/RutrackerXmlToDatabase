namespace RutrackerXmlToDatabase.Core.Models
{
    public class File : BaseModel<long>
    {
        public long Size { get; set; }
        public string Name { get; set; }
        public long TorrentId { get; set; }

        public Torrent Torrent { get; set; }
    }
}