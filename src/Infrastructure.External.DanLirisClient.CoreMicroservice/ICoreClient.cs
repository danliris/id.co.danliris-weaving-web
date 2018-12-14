using System.Threading.Tasks;

namespace Infrastructure.External.DanLirisClient.CoreMicroservice
{
    public interface ICoreClient
    {
        Task<dynamic> RetrieveUnitDepartments();
    }
}