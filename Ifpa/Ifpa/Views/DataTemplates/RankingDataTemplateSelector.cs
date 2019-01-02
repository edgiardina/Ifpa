using Ifpa.Models;
using Ifpa.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ifpa.Views.DataTemplates
{
    class RankingDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate RankingOverallTemplate { get; set; }
        public DataTemplate RankingTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item != null)
            {
                return (container.BindingContext as RankingsViewModel).IsOverallRankings ? RankingOverallTemplate : RankingTemplate;
            }
            return null;
        }
    }
}
