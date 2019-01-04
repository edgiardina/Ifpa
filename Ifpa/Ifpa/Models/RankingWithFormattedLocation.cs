﻿using PinballApi.Models.WPPR.Rankings;

namespace Ifpa.Models
{
    /// <summary>
    /// this class is an unforunate hack to work around missing location data. 
    /// this way if we are missing a field like State or City we won't have extraneous spaces. 
    /// </summary>
    public class RankingWithFormattedLocation : Ranking
    {

        public RankingWithFormattedLocation(Ranking item)
        {

            BestFinish = item.BestFinish;
            EventCount = item.EventCount;
            EfficiencyPercent = item.EfficiencyPercent;
            RatingValue = item.RatingValue;
            LastMonthRank = item.LastMonthRank;
            CurrentWpprRank = item.CurrentWpprRank;
            WpprPoints = item.WpprPoints;
            City = item.City;
            State = item.State;
            CountryCode = item.CountryCode;
            CountryName = item.CountryName;
            Age = item.Age;
            LastName = item.LastName;
            FirstName = item.FirstName;
            PlayerId = item.PlayerId;
            BestFinishPosition = item.BestFinishPosition;
            BestTournamentId = item.BestTournamentId;
        }

        //Replace call at the end so that if a player is missing the 'state' we don't have an unsightly double space.
        public string Location => $"{City} {State} {CountryName}".Trim().Replace("  ", " ");
    }
}