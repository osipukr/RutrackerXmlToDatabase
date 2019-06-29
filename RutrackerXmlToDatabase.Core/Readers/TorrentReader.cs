using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;
using RutrackerXmlToDatabase.Core.Models;
using RutrackerXmlToDatabase.Core.Readers.Extensions;
using File = System.IO.File;

namespace RutrackerXmlToDatabase.Core.Readers
{
    public class TorrentReader : IDisposable
    {
        private readonly FileStream _fileStream;
        private readonly GZipStream _gZipStream;
        private readonly XmlReader _xmlReader;

        public TorrentReader(string path)
        {
            _fileStream = File.OpenRead(path);
            _gZipStream = new GZipStream(_fileStream, CompressionMode.Decompress);
            _xmlReader = XmlReader.Create(_gZipStream, new XmlReaderSettings
            {
                IgnoreComments = true
            });
        }

        public bool EOF => _xmlReader.EOF;

        public IEnumerable<Torrent> Read(int maxCount) => _xmlReader.ReadXElements(maxCount).ReadTorrents();

        public void Dispose()
        {
            _xmlReader.Dispose();
            _gZipStream.Dispose();
            _fileStream.Dispose();
        }
    }
}