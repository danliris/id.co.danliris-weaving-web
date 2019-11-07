using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Warping.Queries
{
    public interface IDailyOperationWarpingReportQuery<TModel>
    {
        Task<IEnumerable<TModel>> GetAll();
        Task<IEnumerable<TModel>> GetByOrder(Guid orderId);
        Task<IEnumerable<TModel>> GetByWeavingUnit(int weavingUnitId);
        Task<IEnumerable<TModel>> GetByMaterialType(Guid materialTypeId);
        Task<IEnumerable<TModel>> GetByDateRange(DateTimeOffset startDate, DateTimeOffset endDate);
        Task<IEnumerable<TModel>> GetByOperationStatus(string operationStatus);
        Task<IEnumerable<TModel>> GetByOrderWeavingUnit(Guid orderId, int weavingUnitId);
        Task<IEnumerable<TModel>> GetByOrderMaterialType(Guid orderId, Guid materialTypeId);
        Task<IEnumerable<TModel>> GetByOrderDateRange(Guid orderId, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<IEnumerable<TModel>> GetByOrderOperationStatus(Guid orderId, string operationStatus);
        Task<IEnumerable<TModel>> GetByWeavingUnitMaterialType(int weavingUnitId, Guid materialTypeId);
        Task<IEnumerable<TModel>> GetByWeavingUnitDateRange(int weavingUnitId, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<IEnumerable<TModel>> GetByWeavingUnitOperationStatus(int weavingUnitId, string operationStatus);
        Task<IEnumerable<TModel>> GetByMaterialTypeDateRange(Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<IEnumerable<TModel>> GetByMaterialTypeOperationStatus(Guid materialTypeId, string operationStatus);
        Task<IEnumerable<TModel>> GetByDateRangeOperationStatus(DateTimeOffset startDate, DateTimeOffset endDate, string operationStatus);
        Task<IEnumerable<TModel>> GetByOrderWeavingUnitMaterialType(Guid orderId, int weavingUnitId, Guid materialTypeId);
        Task<IEnumerable<TModel>> GetByOrderWeavingUnitDateRange(Guid orderId, int weavingUnitId, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<IEnumerable<TModel>> GetByOrderWeavingUnitOperationStatus(Guid orderId, int weavingUnitId, string operationStatus);
        Task<IEnumerable<TModel>> GetByOrderMaterialTypeDateRange(Guid orderId, Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<IEnumerable<TModel>> GetByOrderMaterialTypeOperationStatus(Guid orderId, Guid materialTypeId, string operationStatus);
        Task<IEnumerable<TModel>> GetByOrderDateRangeOperationStatus(Guid orderId, DateTimeOffset startDate, DateTimeOffset endDate, string operationStatus);
        Task<IEnumerable<TModel>> GetByWeavingUnitMaterialTypeDateRange(int weavingUnitId, Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate);
        Task<IEnumerable<TModel>> GetByWeavingUnitMaterialTypeOperationStatus(int weavingUnitId, Guid materialTypeId, string operationStatus);
        Task<IEnumerable<TModel>> GetByMaterialTypeDateRangeOperationStatus(Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate, string operationStatus);
        Task<IEnumerable<TModel>> GetAllSpecified(Guid orderId, int weavingUnitId, Guid materialTypeId, DateTimeOffset startDate, DateTimeOffset endDate, string operationStatus);
    }
}
