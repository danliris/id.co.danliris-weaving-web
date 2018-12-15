using Moonlay.ExtCore.Mvc.Abstractions;

namespace Infrastructure
{
    public interface IWebApiContext : IWorkContext
    {
        string ApiVersion { get; }
    }
}
