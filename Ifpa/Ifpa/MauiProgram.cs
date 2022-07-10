using Ifpa.ViewModels;
using Ifpa.Models;

namespace Ifpa
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("AmaticSC-Regular.ttf", "Amatic");
                    fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialIcons");
                    fonts.AddFont("Michella-Garden.otf", "Michella");
                })
                .Services.AddViewModels<BaseViewModel>();

            //TODO: configure platform specific services
            //TODO: change viewmodels in all pages so that DI creates them.

            return builder.Build();
        }
    }
}
