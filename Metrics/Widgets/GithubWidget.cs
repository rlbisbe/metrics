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
    class GithubWidget : Widget
    {
        public string User { get; set; }
        public string Repository { get; set; }

        public GithubWidget(string User, string Repository)
        {
            this.User = User;
            this.Repository = Repository;
            this.Title = Repository + " open issues";
            this.Background = "#ffffff";
            this.Foreground = "black";
        }

        public override async Task Update()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri("https://api.github.com/repos/" + User + "/" + Repository));
            var result = await response.Content.ReadAsStringAsync();

            // Parse the JSON recipe data
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
