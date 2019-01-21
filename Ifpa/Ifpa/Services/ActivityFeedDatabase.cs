﻿using Ifpa.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ifpa.Services
{
    public class ActivityFeedDatabase
    {
        readonly SQLiteAsyncConnection database;
        public ActivityFeedDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<ActivityFeedItem>().Wait();
        }

        public Task<List<ActivityFeedItem>> GetActivityFeedRecords()
        {
            return database.Table<ActivityFeedItem>().OrderByDescending(n => n.CreatedDateTime).ToListAsync();
        }

        public async Task<int> CreateActivityFeedRecord(ActivityFeedItem item)
        {
            return await database.InsertAsync(item);
        }

        public async Task UpdateActivityFeedRecord(ActivityFeedItem item)
        {
            if(item.ID > 0)
            {
                await database.UpdateAsync(item);
            }
        }

        public async Task ClearActivityFeed()
        {
            await database.DeleteAllAsync<ActivityFeedItem>();
        }

        public async Task<int> GetUnreadActivityCount()
        {
            return await database.Table<ActivityFeedItem>().CountAsync(n => !n.HasBeenSeen);
        }

        public async Task<IEnumerable<int>> ParseNewTournaments(IEnumerable<int> tournamentIds)
        {
            var tournamentsSeen = await database.Table<ActivityFeedItem>().Where(n => n.ActivityType == ActivityFeedType.TournamentResult).ToListAsync();
            return tournamentIds.Except(tournamentsSeen.Select(n => n.RecordID.Value));
        }
    }
}
