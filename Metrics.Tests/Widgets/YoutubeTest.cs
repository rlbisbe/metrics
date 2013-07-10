using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Metrics.Widgets;
using System.Threading.Tasks;
using System.Threading;
using Metrics.Library.Widgets;

namespace Metrics.Test.Widgets
{
    [TestClass]
    public class YoutubeTest
    {
        [TestMethod]
        public void TestYoutubeWidget()
        {
            YoutubeWidget test
                = new YoutubeWidget("foo",
                    YoutubeWidget.Selection.ViewCount);
            test.HttpService = new MockHttpService("YoutubeJson.txt");

            test.Update().Wait();
            Assert.AreEqual(100, test.Counter);

        }
    }
}
