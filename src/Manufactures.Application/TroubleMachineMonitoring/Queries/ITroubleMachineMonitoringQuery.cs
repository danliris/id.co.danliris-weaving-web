using Manufactures.Application.TroubleMachineMonitoring.DTOs;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.TroubleMachineMonitoring.Queries
{
    public interface ITroubleMachineMonitoringQuery
    {
        Task <IEnumerable<TroubleMachineMonitoringListDto>> GetAll();
        Task <TroubleMachineMonitoringDto> GetById(Guid id);
    }
}
