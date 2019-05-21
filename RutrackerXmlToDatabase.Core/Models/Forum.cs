using System.Collections.Generic;

namespace RutrackerXmlToDatabase.Core.Models
{
    public class Forum : BaseModel<long>
    {
        public string Title { get; set; }

        public ICollection<Torrent> Torrents { get; set; }
    }
}