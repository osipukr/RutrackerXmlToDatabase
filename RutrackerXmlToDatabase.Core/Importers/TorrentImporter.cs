﻿using Microsoft.EntityFrameworkCore;
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
        /// <param name="readCount">Maximum number of entities to read at a time.</param>
        /// <exception cref="ArgumentException"></exception>
        public static async Task ImportAsync(string connectionString, string filePath, int readCount = 5000)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Not a valid connection string.", nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("Invalid file path.", nameof(filePath));
            }

            if (readCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(readCount),
                    "The maximum number of elements must be greater than 1.");
            }

            var dbOptions = new DbContextOptionsBuilder<TorrentDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            void BulkOptions(BulkOperation<Torrent> options)
            {
                options.BatchSize = readCount;
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

            EntityFrameworkManager.ContextFactory = x => new TorrentDbContext(dbOptions);

            using var context = new TorrentDbContext(dbOptions);
            using var reader = new TorrentReader(filePath);

            await context.Database.EnsureCreatedAsync();

            while (!reader.EOF)
            {
                var torrents = reader.Read(readCount);

                await context.BulkInsertAsync(torrents, BulkOptions);
                await context.BulkSaveChangesAsync();
            }
        }
    }
}