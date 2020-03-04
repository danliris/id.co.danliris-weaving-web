using Manufactures.Domain.DailyOperations.Sizing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.SizePickupReport
{
    public class SizePickupReportListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty(PropertyName = "RecipeCode")]
        public string RecipeCode { get; set; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "TexSQ")]
        public int TexSQ { get; set; }

        [JsonProperty(PropertyName = "Visco")]
        public int Visco { get; set; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; set; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; set; }

        [JsonProperty(PropertyName = "DateTimeMachineHistory")]
        public DateTimeOffset DateTimeMachineHistory { get; set; }

        [JsonProperty(PropertyName = "StartCounter")]
        public double CounterStart { get; set; }

        [JsonProperty(PropertyName = "FinishCounter")]
        public double CounterFinish { get; set; }

        [JsonProperty(PropertyName = "NettoWeight")]
        public double WeightNetto { get; set; }

        [JsonProperty(PropertyName = "BrutoWeight")]
        public double WeightBruto { get; set; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; set; }

        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }


        public SizePickupReportListDto(DailyOperationSizingDocument document, 
                                       string operatorName, 
                                       string operatorGroup, 
                                       DateTimeOffset dateTimeDoff, 
                                       double counterStart, 
                                       double counterFinish, 
                                       double weightNetto, 
                                       double weightBruto, 
                                       double pisMeter, 
                                       double spu, 
                                       string beamNumber,
                                       string category,
                                       string orderNumber)
        {
            Id = document.Identity;
            RecipeCode = document.RecipeCode;
            MachineSpeed = document.MachineSpeed;
            TexSQ = document.TexSQ;
            Visco = document.Visco;
            OperatorName = operatorName;
            OperatorGroup = operatorGroup;
            DateTimeMachineHistory = dateTimeDoff;
            CounterStart = counterStart;
            CounterFinish = counterFinish;
            WeightNetto = weightNetto;
            WeightBruto = weightBruto;
            PISMeter = pisMeter;
            SPU = spu;
            BeamNumber = beamNumber;
            Category = category;
            OrderNumber = orderNumber;
        }

        public SizePickupReportListDto()
        {
        }
    }
}
