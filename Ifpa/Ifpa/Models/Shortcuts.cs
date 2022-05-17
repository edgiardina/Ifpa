using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ifpa.Models
{
    public static class Shortcuts
    {
        public const string AppShortcutUriBase = "ifpa://ifpacompanion";

        public static List<Shortcut> ShortcutList = new List<Shortcut>
                                                    {
                                                        new Shortcut() {
                                                            Label = "Calendar",
                                                            Description = "IFPA Tournament Calendar",
                                                            Icon = new DateIcon(),
                                                            Uri = $"{AppShortcutUriBase}/calendar"
                                                        },
                                                        new Shortcut() {
                                                            Label = "My Stats",
                                                            Description = "Your IFPA player data",
                                                            Icon = new ContactIcon(),
                                                            Uri = $"{AppShortcutUriBase}/my-stats"
                                                        },
                                                        new Shortcut() {
                                                            Label = "Player Search",
                                                            Description = "Search for other players in the IFPA database",
                                                            Icon = new SearchIcon(),
                                                            Uri = $"{AppShortcutUriBase}/rankings/player-search"
                                                        }
                                                    };

        public static async Task AddShortcuts()
        {
            if (CrossAppShortcuts.IsSupported)
            {
                var existingShortCurts = await CrossAppShortcuts.Current.GetShortcuts();

                foreach (var t in ShortcutList)
                {
                    if(!existingShortCurts.Any(n => n.Label == t.Label))
                        await CrossAppShortcuts.Current.AddShortcut(t);
                }
            }
        }

    }
}
