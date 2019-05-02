using RutrackerXmlToDatabase.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace RutrackerXmlToDatabase.Core.Readers.Extensions
{
    public static class TorrentReaderExtension
    {
        public static IEnumerable<XElement> ReadXElements(this XmlReader reader, int maxCount)
        {
            var count = 0;

            while (count != maxCount && reader.Read())
            {
                if (!(reader.NodeType == XmlNodeType.Element
                        && reader.Name == "torrent"
                        && reader.AttributeCount == 3))
                {
                    continue;
                }

                if (XNode.ReadFrom(reader) is XElement element)
                {
                    yield return element;

                    count++;
                }
            }
        }

        public static IEnumerable<Torrent> ReadTorrents(this IEnumerable<XElement> torrents)
        {
            return torrents.Select(t =>
            {
                long.TryParse(t.Attribute("size").Value, out long size);

                return new Torrent()
                {
                    Id = (long)t.Attribute("id"),
                    Date = DateTime.Parse((string)t.Attribute("registred_at")),
                    Size = size,
                    Title = (string)t.Element("title"),
                    Hash = (string)t.Element("torrent").Attribute("hash"),
                    TrackerId = (long)t.Element("torrent").Attribute("tracker_id"),
                    ForumId = (long)t.Element("forum").Attribute("id"),
                    ForumTitle = (string)t.Element("forum"),
                    IsDeleted = t.Element("del") != null,
                    Content = (string)t.Element("content"),
                    DupConfidence = (int?)t.Element("dup")?.Attribute("p"),
                    DupTorrentId = (long?)t.Element("dup")?.Attribute("id"),
                    DupTitile = (string)t.Element("dup"),
                    Files = t.ReadTorrentFiles().ToArray()
                };
            });
        }

        private static IEnumerable<File> ReadTorrentFiles(this XElement torrent)
        {
            var torrentId = (long)torrent.Attribute("id");

            return torrent.Elements("file").Select(f => new File()
            {
                Size = (long)f.Attribute("size"),
                Name = (string)f.Attribute("name"),
                TorrentId = torrentId
            });
        }
    }
}