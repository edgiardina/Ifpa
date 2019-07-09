using HtmlAgilityPack;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ifpa.ViewModels
{
    public class RulesViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public HtmlWebViewSource RulesContent { get; set; }

        private const string RulesPdfUrl = "https://papa.org/wp-content/uploads/Professional-Amateur-Pinball-Association-Complete-Competition-Rules-5.pdf";

        private Stream pdfDocumentStream;

        public Stream PdfDocumentStream
        {
            get { return pdfDocumentStream; }
            set
            {
                //Check the value whether it is the same as the currently loaded stream
                if (value != pdfDocumentStream)
                {
                    pdfDocumentStream = value;
                    OnPropertyChanged("PdfDocumentStream");
                }
            }
        }

        private async Task<Stream> DownloadPdfStream(string URL)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(URL);
            //Check whether redirection is needed
            if ((int)response.StatusCode == 302)
            {
                //The URL to redirect is in the header location of the response message
                HttpResponseMessage redirectedResponse = await httpClient.GetAsync(response.Headers.Location.AbsoluteUri);
                return await redirectedResponse.Content.ReadAsStreamAsync();
            }
            return await response.Content.ReadAsStreamAsync();
        }

    
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
                PdfDocumentStream = await DownloadPdfStream(RulesPdfUrl);
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
