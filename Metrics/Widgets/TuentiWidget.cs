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
    class TuentiWidget : Widget
    {
        public string Source { get; set; }

        public TuentiWidget(string Source)
        {
            this.Source = Source;
            this.Title = Source + " likes";
            this.Background = "#003b6a";
            this.Foreground = "#8ec9e8";
        }

        public override async Task Update()
        {
            //TODO: Controlar si esto explota. De hecho, probar con sonypicturesspain
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri("http://tuenti.com/" + Source));
            var result = await response.Content.ReadAsStringAsync();
            var numero = Regex.Match(result, @"<li \s*(?:(?:\b(\w|-)+\b\s*(?:=\s*(?:""[^""]*""|'[^']*'|[^""'<> ]+)\s*)?)*)/?\s*>(\d\.)*(\d)*").Value.Replace(".","").Substring(28);
            Counter = int.Parse(numero);       
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "FacebookWidget";
            composite["source"] = Source;
            return composite;
        }
    }
}
