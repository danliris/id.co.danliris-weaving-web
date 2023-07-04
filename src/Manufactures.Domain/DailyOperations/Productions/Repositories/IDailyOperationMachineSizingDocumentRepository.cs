using Infrastructure.Domain.Repositories;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Productions.Repositories
{
    public interface IDailyOperationMachineSizingDocumentRepository : IAggregateRepository<DailyOperationMachineSizingDocument, DailyOperationMachineSizingDocumentReadModel>
    {
        //Task<string> GetEstimationNumber();
    }
}
