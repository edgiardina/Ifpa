using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ifpa.Effects
{
    public class TintImageEffect : RoutingEffect
    {
        public const string GroupName = "MyCompany";
        public const string Name = "TintImageEffect";

        public Color TintColor { get; set; }

        public TintImageEffect() : base($"{GroupName}.{Name}") { }
    }
}
