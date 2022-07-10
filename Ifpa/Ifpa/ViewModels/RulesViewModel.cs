using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui;

namespace Ifpa.ViewModels
{
    public class RulesViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public HtmlWebViewSource RulesContent { get; set; }

        private const string RulesPdfUrl = "https://www.ifpapinball.com/wp/wp-content/uploads/2021/04/PAPA_IFPA-Complete-Competition-Rules-2021.04.06.pdf";

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
