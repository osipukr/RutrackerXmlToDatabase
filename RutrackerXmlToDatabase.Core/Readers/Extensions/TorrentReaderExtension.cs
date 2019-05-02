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
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var count = 0;

            while (count != maxCount && reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element
                        || reader.Name != "torrent"
                        || reader.AttributeCount != 3)
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
            if (torrents == null)
            {
                throw new ArgumentNullException(nameof(torrents));
            }

            return torrents.Select(x =>
            {
                long.TryParse(x.Attribute("size").Value, out long size);

                return new Torrent()
                {
                    Id = (long)x.Attribute("id"),
                    Date = DateTime.Parse((string)x.Attribute("registred_at")),
                    Size = size,
                    Title = (string)x.Element("title"),
                    Hash = (string)x.Element("torrent").Attribute("hash"),
                    TrackerId = (long)x.Element("torrent").Attribute("tracker_id"),
                    ForumId = (long)x.Element("forum").Attribute("id"),
                    ForumTitle = (string)x.Element("forum"),
                    IsDeleted = x.Element("del") != null,
                    Content = (string)x.Element("content"),
                    DupConfidence = (int?)x.Element("dup")?.Attribute("p"),
                    DupTorrentId = (long?)x.Element("dup")?.Attribute("id"),
                    DupTitile = (string)x.Element("dup"),
                    Files = x.ReadTorrentFiles().ToArray()
                };
            });
        }

        private static IEnumerable<File> ReadTorrentFiles(this XElement torrent)
        {
            if (torrent == null)
            {
                throw new ArgumentNullException(nameof(torrent));
            }

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