using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.Services
{
    public class LocalizationService
    {
        public static string GetString(string key)
        {
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (loader == null)
                return string.Empty;
            
            return loader.GetString(key);
        }
    }
}
