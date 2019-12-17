using Ifpa.Styles;
using System.Threading.Tasks;

namespace Ifpa.Interfaces
{
    public interface IThemeInspector
    {
        AppTheme GetOperatingSystemTheme();
        Task<AppTheme> GetOperatingSystemThemeAsync();
    }
}
