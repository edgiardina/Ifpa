using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class RulesViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public HtmlWebViewSource RulesContent { get; set; }

        public RulesViewModel()
        {
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                RulesContent = new HtmlWebViewSource();
                var url = "https://www.ifpapinball.com/rules/";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var rulesHtmlContent = doc.DocumentNode.Descendants("div")
                                         .First(x => x.Attributes.AttributesWithName("class").Any() && x.Attributes["class"].Value == "postblock");                                       

                //TODO: make this HTML and CSS in discrete files 
                RulesContent.Html = "<html><head><style>img {display:block; clear:both;  margin: auto; margin-botton:10px !important;}</style></head><body style='font-family:sans-serif;'>" + rulesHtmlContent.InnerHtml + "</body></html>";
                OnPropertyChanged(nameof(RulesContent));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
