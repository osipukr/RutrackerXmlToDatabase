using Microsoft.EntityFrameworkCore;
using RutrackerXmlToDatabase.Core.Contexts;
using RutrackerXmlToDatabase.Core.Models;
using RutrackerXmlToDatabase.Core.Readers;
using System;
using System.Threading.Tasks;
using Z.BulkOperations;
using Z.EntityFramework.Extensions;

namespace RutrackerXmlToDatabase.Core.Importers
{
    /// <summary>
    /// Class for importing Torrent entities.
    /// </summary>
    public static class TorrentImporter
    {
        /// <summary>
        /// Importing data from a file into a database.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="filePath">The path to the file to import.</param>
        /// <param name="maxCount">Maximum number of entities to read at a time.</param>
        /// <exception cref="ArgumentException"></exception>
        public static async Task ImportAsync(string connectionString, string filePath, int maxCount = 5000)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Not a valid connection string.", nameof(connectionString));
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid file path.", nameof(filePath));
            }

            if (maxCount <= 0)
            {
                throw new ArgumentException("The maximum number of elements must be greater than 0.", nameof(maxCount));
            }

            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            void BulkOptions(BulkOperation<Torrent> options)
            {
                options.BatchSize = maxCount;
                options.IncludeGraph = true;
                options.IncludeGraphOperationBuilder = operation =>
                {
                    if (operation is BulkOperation<Forum> forumOperation)
                    {
                        forumOperation.InsertIfNotExists = true;
                        forumOperation.ColumnPrimaryKeyExpression = x => x.Id;
                    }
                };
            }

            EntityFrameworkManager.ContextFactory = context => new AppDbContext(dbOptions);

            using (var context = new AppDbContext(dbOptions))
            using (var reader = new TorrentReader(filePath))
            {
                while (!reader.EOF)
                {
                    var torrents = reader.Read(maxCount);

                    await context.BulkInsertAsync(torrents, BulkOptions);
                    await context.BulkSaveChangesAsync();
                }
            }
        }
    }
}