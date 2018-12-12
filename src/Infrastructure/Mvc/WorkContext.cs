using Moonlay.ExtCore.Mvc.Abstractions;

namespace Infrastructure
{
    public class WorkContext : IWorkContext
    {
        public string CurrentUser { get; internal set; }
    }
}
