using Infrastructure;
using System.Diagnostics;

namespace DanLiris.Admin.Web
{
    public class WorkContext : IWebApiContext
    {
        public WorkContext()
        {
            var assembly = typeof(WorkContext).Assembly;
            var version = FileVersionInfo.GetVersionInfo(assembly.Location);

            ApiVersion = version.FileVersion;
        }

        public string CurrentUser { get; internal set; }

        public string ApiVersion { get; internal set; }
    }
}
