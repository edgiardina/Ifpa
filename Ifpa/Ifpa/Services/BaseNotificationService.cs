﻿using Ifpa.Models;
using Ifpa.ViewModels;
using PinballApi;
using Plugin.Badge;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.Services
{
    public abstract class BaseNotificationService
    {
        private PinballRankingApiV1 PinballRankingApi => new PinballRankingApiV1(Constants.IfpaApiKey);

        public readonly string NewTournamentNotificationTitle = "New Tournament Result";
        protected readonly string NewTournamentNotificationDescription = "A new tournament result has been posted to your IFPA profile";

        public readonly string NewRankNotificationTitle = "IFPA Rank Change";
        protected readonly string NewRankNotificationDescription = "Your IFPA rank has changed!";

        public static string NewBlogPostTitle = "New Blog Post";
        protected readonly string NewBlogPostDescription = "A new IFPA blog post has been published";

        public async Task NotifyIfUserHasNewlySubmittedTourneyResults()
        {
            if (Settings.HasConfiguredMyStats)
            {
                try
                {
                    var results = await PinballRankingApi.GetPlayerResults(Settings.MyStatsPlayerId);
                    
                    var unseenTournaments = await Settings.FindUnseenTournaments(results.Results);

                    if (unseenTournaments.Any())
                    {
                        // Five here is a proxy for 
                        // "did we switch users and therefore are adding all historical events to activity log"
                        // We need historical data to know when a user should get alerted due to a new tournament
                        var isHistoricalEventPopulation = unseenTournaments.Count() >= 5;

                        if (Settings.NotifyOnTournamentResult && !isHistoricalEventPopulation)
                        {
                            SendNotification(NewTournamentNotificationTitle, NewTournamentNotificationDescription);
                            await UpdateBadgeIfNeeded();
                        }

                        foreach (var unseenTournament in unseenTournaments.OrderBy(n => n))
                        {
                            var result = results.Results.Single(n => n.TournamentId == unseenTournament);

                            var record = new ActivityFeedItem
                            {
                                CreatedDateTime = isHistoricalEventPopulation ? result.EventDate : DateTime.Now,
                                HasBeenSeen = isHistoricalEventPopulation,
                                RecordID = result.TournamentId,
                                IntOne = result.Position,
                                ActivityType = ActivityFeedType.TournamentResult
                            };

                            await Settings.LocalDatabase.CreateActivityFeedRecord(record);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task NotifyIfUsersRankChanged()
        {
            if (Settings.HasConfiguredMyStats)
            {
                try
                {
                    var results = await PinballRankingApi.GetPlayerRecord(Settings.MyStatsPlayerId);

                    var currentWpprRank = results.PlayerStats.CurrentWpprRank;
                    var lastRecordedWpprRank = Settings.MyStatsCurrentWpprRank;

                    if (currentWpprRank != lastRecordedWpprRank && lastRecordedWpprRank != 0)
                    {
                        if (Settings.NotifyOnRankChange)
                        {
                            SendNotification(NewRankNotificationTitle, NewRankNotificationDescription);
                            await UpdateBadgeIfNeeded();
                        }

                        var record = new ActivityFeedItem
                        {
                            CreatedDateTime = DateTime.Now,
                            HasBeenSeen = false,
                            IntOne = currentWpprRank,
                            IntTwo = lastRecordedWpprRank,
                            ActivityType = ActivityFeedType.RankChange
                        };

                        Settings.MyStatsCurrentWpprRank = currentWpprRank;

                        await Settings.LocalDatabase.CreateActivityFeedRecord(record);                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task NotifyIfNewBlogItemPosted()
        {
            if(Settings.NotifyOnNewBlogPost)
            {
                try
                {
                    var blogPostService = new BlogPostService();

                    var latestPosts = await blogPostService.GetBlogPosts();

                    var latestGuidInPosts = blogPostService.ParseGuidFromInternalId(latestPosts.First().InternalID);

                    if (latestGuidInPosts > Settings.LastBlogPostGuid)
                    {
                        if(Settings.LastBlogPostGuid > 0)
                        {
                            SendNotification(NewBlogPostTitle, NewBlogPostDescription);
                        }

                        Settings.LastBlogPostGuid = latestGuidInPosts;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task UpdateBadgeIfNeeded()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var unreads = await Settings.LocalDatabase.GetUnreadActivityCount();
                CrossBadge.Current.SetBadge(unreads);
            }
        }

        public abstract void SendNotification(string title, string description);
    }
}
