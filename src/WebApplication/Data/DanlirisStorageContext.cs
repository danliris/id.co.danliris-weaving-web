using ExtCore.Data.EntityFramework;
using ExtCore.Data.EntityFramework.SqlServer;
using Microsoft.Extensions.Options;

namespace DanLiris.Admin.Web
{
    public class DanlirisStorageContext : StorageContext
    {
        public DanlirisStorageContext(IOptions<StorageContextOptions> options) : base(options)
        {
        }
    }
}