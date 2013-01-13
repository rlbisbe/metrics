using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Metrics
{
    public class NetworkService
    {
        public static bool HaveInternetConnection()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile == null)
                return false;

            var level = profile.GetNetworkConnectivityLevel();
            if (level == NetworkConnectivityLevel.LocalAccess
                || level == NetworkConnectivityLevel.None)
                return false;

            return true;
        }
    }
}
