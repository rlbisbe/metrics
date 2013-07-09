using Metrics.Services;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Metrics.Test.Widgets
{
    class MockHttpService : IHttpService
    {
        public MockHttpService(string fileName)
        {
            mFileName = fileName;
        }

        public async Task<JsonObject> GetJsonResult(string url)
        {
            var path = @"Widgets\" + mFileName;
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            var file = await folder.GetFileAsync(path);
            Assert.IsNotNull(file, "Acquire file");
            var content = await Windows.Storage.FileIO.ReadTextAsync(file);

            JsonObject result = JsonObject.Parse(content);
            return result;
        }

        string mFileName;
    }
}
