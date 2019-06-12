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
                        && reader.AttributeCount == 3
                        && XNode.ReadFrom(reader) is XElement element))
                {
                    continue;
                }

                yield return element;

                count++;
            }
        }

        public static IEnumerable<Torrent> ReadTorrents(this IEnumerable<XElement> torrents)
        {
            return torrents.Select(t =>
            {
                long.TryParse((string)t.Attribute("size"), out var size);

                var torrent = t.Element("torrent");
                var dup = t.Element("dup");

                return new Torrent()
                {
                    Id = (long)t.Attribute("id"),
                    Date = DateTime.Parse((string)t.Attribute("registred_at")),
                    Size = size,
                    Title = (string)t.Element("title"),
                    Hash = (string)torrent?.Attribute("hash"),
                    TrackerId = (long)torrent?.Attribute("tracker_id"),
                    Forum = t.ReadTorrentForum(),
                    IsDeleted = t.Element("del") != null,
                    Content = (string)t.Element("content"),
                    DupConfidence = (int?)dup?.Attribute("p"),
                    DupTorrentId = (long?)dup?.Attribute("id"),
                    DupTitle = (string)dup,
                    Files = t.ReadTorrentFiles().ToArray()
                };
            });
        }

        private static Forum ReadTorrentForum(this XElement torrent)
        {
            var forum = torrent.Element("forum");

            return new Forum()
            {
                Id = (long)forum?.Attribute("id"),
                Title = (string)forum
            };
        }

        private static IEnumerable<File> ReadTorrentFiles(this XElement torrent)
        {
            var torrentId = (long)torrent.Attribute("id");

            return torrent.Descendants("file").Select(f => new File()
            {
                Size = (long)f.Attribute("size"),
                Name = (string)f.Attribute("name"),
                TorrentId = torrentId
            });
        }
    }
}