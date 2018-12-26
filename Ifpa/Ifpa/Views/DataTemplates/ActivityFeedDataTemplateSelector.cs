﻿using Ifpa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ifpa.Views.DataTemplates
{
    class ActivityFeedDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RankChangeTemplate { get; set; }
        public DataTemplate TournamentResultTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item != null)
            {
                return ((ActivityFeedItem)item).ActivityType == ActivityFeedType.RankChange ? RankChangeTemplate : TournamentResultTemplate;
            }
            return null;
        }
    }
}
