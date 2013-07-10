using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace Metrics.Widgets
{
    /// <summary>
    /// Github Widget
    /// </summary>
    /// API Info: http://developer.github.com/v3/
    public class GithubWidget : Widget
    {
        public string User { get; set; }
        public string Repository { get; set; }

        public GithubWidget(string User, string Repository)
        {
            this.User = User;
            this.Repository = Repository;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            this.Title = String.Format(loader.GetString("GithubWidgetOpenIssues"), this.Repository);
            this.Background = "#ffffff";
            this.Foreground = "black";
            this.WidgetForeground = "#33000000";
            this.WidgetName = "github";
        }

        public override async Task Update()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri("https://api.github.com/repos/" + User + "/" + Repository));
            var result = await response.Content.ReadAsStringAsync();

            // Parse the JSON recipe data
            if (result.Contains("API Rate Limit"))
                return;
            
            var recipes = JsonObject.Parse(result);
            Counter = (int)recipes["open_issues"].GetNumber();
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "GithubWidget";
            composite["repository"] = Repository;
            composite["username"] = User;
            return composite;
        }
    }
}
