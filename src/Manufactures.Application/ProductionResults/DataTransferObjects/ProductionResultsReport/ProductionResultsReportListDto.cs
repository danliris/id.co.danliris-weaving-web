using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.ProductionResults.DataTransferObjects.ProductionResultsReport
{
    public class ProductionResultsReportListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public string WeavingUnit { get; set; }

        [JsonProperty(PropertyName = "OrderProductionNumber")]
        public string OrderProductionNumber { get; set; }

        [JsonProperty(PropertyName = "ProductionResultDate")]
        public DateTimeOffset ProductionResultDate { get; set; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "Production")]
        public double Production { get; set; }

        [JsonProperty(PropertyName = "SCMPX")]
        public double SCMPX { get; set; }

        [JsonProperty(PropertyName = "Efficiency")]
        public double Efficiency { get; set; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public double MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "WeftBrokenThreads")]
        public double WeftBrokenThreads { get; set; }

        [JsonProperty(PropertyName = "WarpBrokenThreads")]
        public double WarpBrokenThreads { get; set; }

        [JsonProperty(PropertyName = "BrokenThreadsTotal")]
        public double BrokenThreadsTotal { get; set; }

        [JsonProperty(PropertyName = "LostTime")]
        public TimeSpan LostTime { get; set; }

        [JsonProperty(PropertyName = "ProductionTime")]
        public TimeSpan ProductionTime { get; set; }

        public ProductionResultsReportListDto(Guid id, 
                                              string weavingUnit, 
                                              string orderProductionNumber, 
                                              DateTimeOffset productionResultDate, 
                                              string machineNumber, 
                                              double production, 
                                              double sCMPX, 
                                              double efficiency, 
                                              double machineSpeed, 
                                              double weftBrokenThreads, 
                                              double warpBrokenThreads, 
                                              double brokenThreadsTotal, 
                                              TimeSpan lostTime, 
                                              TimeSpan productionTime)
        {
            Id = id;
            WeavingUnit = weavingUnit;
            OrderProductionNumber = orderProductionNumber;
            ProductionResultDate = productionResultDate;
            MachineNumber = machineNumber;
            Production = production;
            SCMPX = sCMPX;
            Efficiency = efficiency;
            MachineSpeed = machineSpeed;
            WeftBrokenThreads = weftBrokenThreads;
            WarpBrokenThreads = warpBrokenThreads;
            BrokenThreadsTotal = brokenThreadsTotal;
            LostTime = lostTime;
            ProductionTime = productionTime;
        }

    }
}
