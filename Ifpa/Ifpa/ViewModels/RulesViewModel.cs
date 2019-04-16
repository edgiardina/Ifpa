using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

                var htmlShim = await GetHtmlRulesShim();

                RulesContent.Html = htmlShim.Replace("{rules}", rulesHtmlContent.InnerHtml);
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

        private async Task<string> GetHtmlRulesShim()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(RulesViewModel)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("Ifpa.Content.Rules.html");       
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

    }
}
