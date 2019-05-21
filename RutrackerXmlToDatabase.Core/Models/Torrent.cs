using System;
using System.Collections.Generic;

namespace RutrackerXmlToDatabase.Core.Models
{
    public class Torrent : BaseModel<long>
    {
        public DateTime Date { get; set; }
        public long Size { get; set; }
        public string Title { get; set; }
        public string Hash { get; set; }
        public long TrackerId { get; set; }
        public long ForumId { get; set; }
        public bool IsDeleted { get; set; }
        public string Content { get; set; }
        public int? DupConfidence { get; set; }
        public long? DupTorrentId { get; set; }
        public string DupTitle { get; set; }

        public Forum Forum { get; set; }
        public ICollection<File> Files { get; set; }
    }
}