using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace Metrics.Widgets
{
    /// <summary>
    /// Tuenti Widget
    /// </summary>
    /// Sorry folks, no app here
    class TuentiWidget : Widget
    {
        public string Source { get; set; }

        public TuentiWidget(string Source)
        {
            this.Source = Source;
            this.Title = Source + " likes";
            this.Background = "#003b6a";
            this.Foreground = "#8ec9e8";
            this.WidgetForeground = "#338ec9e8";
            this.WidgetName = "tuenti";
        }

        public override async Task Update()
        {
            //TODO: Controlar si esto explota. De hecho, probar con sonypicturesspain
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri("http://tuenti.com/" + Source));
            var result = await response.Content.ReadAsStringAsync();
            try
            {
                var numero = Regex.Match(result, @"<li \s*(?:(?:\b(\w|-)+\b\s*(?:=\s*(?:""[^""]*""|'[^']*'|[^""'<> ]+)\s*)?)*)/?\s*>(\d)*(\.)*(\d)*").Value.Replace(".", "").Substring(28);
                Counter = int.Parse(numero);
            }
            catch (Exception)
            {
                throw new NullReferenceException("The selected page was not found. Please check spelling.");
            }
              
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "TuentiWidget";
            composite["source"] = Source;
            return composite;
        }
    }
}
