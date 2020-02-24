using ExtCore.Data.Abstractions;
using Manufactures.Application.TroubleMachineMonitoring.DTOs;
using Manufactures.Application.TroubleMachineMonitoring.Queries;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manufactures.Application.TroubleMachineMonitoring.QueryHandlers
{
    public class TroubleMachineMonitoringQueryHandler : ITroubleMachineMonitoringQuery
    {
        private readonly IStorage
               _storage;
        private readonly IFabricConstructionRepository
            _constructionRepository;
        private readonly IOrderRepository
            _orderRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IMachineRepository
            _machineRepository;
        private readonly IMachineTypeRepository
            _machineTypeRepository;

        private readonly ITroubleMachineMonitoringRepository
            _troubleMachineMonitoringRepository;

        public TroubleMachineMonitoringQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            _troubleMachineMonitoringRepository =
                _storage.GetRepository<ITroubleMachineMonitoringRepository>();
            _orderRepository =
               _storage.GetRepository<IOrderRepository>();
            _constructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _machineRepository =
                _storage.GetRepository<IMachineRepository>();
            _machineTypeRepository =
                _storage.GetRepository<IMachineTypeRepository>();
        }


        public async Task<IEnumerable<TroubleMachineMonitoringListDto>> GetAll()
        {
            var resultListDto = new List<TroubleMachineMonitoringListDto>();

            var TroubleMachineMonitoringQuery =
                _troubleMachineMonitoringRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            await Task.Yield();
            var TroubleMachineMonitoringDocuments =
                    _troubleMachineMonitoringRepository
                        .Find(TroubleMachineMonitoringQuery);

            var orderIds = TroubleMachineMonitoringQuery.Select(s => s.OrderDocument).ToList();
            var orders = _orderRepository.Query.Where(w => orderIds.Contains(w.Identity)).ToDictionary(k => k.Identity, v => new { v.OrderNumber, v.UnitId, v.ConstructionDocumentId });

            var operatorIds = TroubleMachineMonitoringQuery.Select(s => s.OperatorDocument).ToList();
            var operatorNames = _operatorRepository.Find(o => operatorIds.Contains(o.Identity)).ToDictionary(k => k.Identity, v => v.CoreAccount.Name);

            var construnctionIds = orders.Values.Select(s => s.ConstructionDocumentId).ToList();
            var constructions = _constructionRepository.Query.Where(w => construnctionIds.Contains(w.Identity)).ToDictionary(k => k.Identity, v => v.ConstructionNumber);

            var machineIds = TroubleMachineMonitoringQuery.Select(s => s.MachineDocument).ToList();
            var machines = _machineRepository.Query.Where(w => machineIds.Contains(w.Identity)).ToDictionary(k => k.Identity, v => new { v.MachineNumber, v.Location, v.MachineTypeId });

            foreach (var troubleMachine in TroubleMachineMonitoringDocuments)
            {
                var resultDto = new TroubleMachineMonitoringListDto(troubleMachine);

                var order = orders.GetValueOrDefault(troubleMachine.OrderDocument.Value);
                var operatorName = operatorNames.GetValueOrDefault(troubleMachine.OperatorDocument.Value);
                var constructionNumber = constructions.GetValueOrDefault(order.ConstructionDocumentId);
                var machine = machines.GetValueOrDefault(troubleMachine.MachineDocument.Value);

                resultDto.SetOrderName(order.OrderNumber); 
                resultDto.SetUnitId(order.UnitId);
                resultDto.SetOperatorName(operatorName);
                resultDto.SetMachineNumber(machine.MachineNumber); 
                resultDto.SetConstructionNumber(constructionNumber);

                resultListDto.Add(resultDto);
            }
            return resultListDto;
        }

        public async Task<TroubleMachineMonitoringDto> GetById(Guid id)
        {
            var query =
                _troubleMachineMonitoringRepository
                    .Query
                    .Where(x => x.Identity.Equals(id))
                    .OrderByDescending(x => x.CreatedDate);

            await Task.Yield();
            var existingTroubleMachineMonitoring =
                _troubleMachineMonitoringRepository.Find(query)
                    .Select(y => new TroubleMachineMonitoringDto(y))
                    .FirstOrDefault();

            var order =
                _orderRepository
                .Find(o => o.Identity == existingTroubleMachineMonitoring.OrderDocument)
                .FirstOrDefault();

            var construction =
                _constructionRepository
                .Find(c => c.Identity == order.ConstructionDocumentId.Value)
                .FirstOrDefault();

            var operatorId =
                _operatorRepository
                .Find(o => o.Identity == existingTroubleMachineMonitoring.OperatorDocument)
                .FirstOrDefault();

            var machine =
                _machineRepository
                .Find(m => m.Identity == existingTroubleMachineMonitoring.MachineDocument)
                .FirstOrDefault();

            var machineType =
               _machineTypeRepository
               .Find(m => m.Identity == machine.MachineTypeId.Value)
               .FirstOrDefault();

            existingTroubleMachineMonitoring.SetOrderNumber(order.OrderNumber);
            existingTroubleMachineMonitoring.SetConstructionNumber(construction.ConstructionNumber);
            existingTroubleMachineMonitoring.SetOperatorName(operatorId.CoreAccount.Name);
            existingTroubleMachineMonitoring.SetMachineLocation(machine.Location);
            existingTroubleMachineMonitoring.SetMachineTypeName(machineType.TypeName);
            existingTroubleMachineMonitoring.SetMachineNumber(machine.MachineNumber);
            existingTroubleMachineMonitoring.SetUnitId(order.UnitId.Value);
            return existingTroubleMachineMonitoring;
        }

       }
}
