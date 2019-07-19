﻿using Manufactures.Domain.DailyOperations.Sizing;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos
{
    public class SizePickupListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "DateTimeMachineHistory")]
        public DateTimeOffset DateTimeMachineHistory { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName{ get; }

        [JsonProperty(PropertyName = "OperatorGroup")]
        public string OperatorGroup { get; }

        [JsonProperty(PropertyName = "RecipeCode")]
        public string RecipeCode { get; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; }

        [JsonProperty(PropertyName = "TexSQ")]
        public double TexSQ { get; }

        [JsonProperty(PropertyName = "Visco")]
        public double Visco { get; }

        [JsonProperty(PropertyName = "PISM")]
        public int PISM { get; }

        //[JsonProperty(PropertyName = "CounterStart")]
        //public string CounterStart { get; }

        //[JsonProperty(PropertyName = "CounterFinish")]
        //public string CounterFinish { get; }

        //[JsonProperty(PropertyName = "WeightNetto")]
        //public string WeightNetto { get; }

        //[JsonProperty(PropertyName = "WeightBruto")]
        //public string WeightBruto { get; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; }

        public SizePickupListDto(DailyOperationSizingDocument document, DateTimeOffset dateTimeOperation, string operatorName, string operatorGroup, string beamNumber)
        {
            Id = document.Identity;
            DateTimeMachineHistory = dateTimeOperation;
            OperatorName= operatorName;
            OperatorGroup = operatorGroup;
            RecipeCode = document.RecipeCode;
            MachineSpeed = document.MachineSpeed;
            TexSQ = document.TexSQ;
            Visco = document.Visco;
            //PISM = document.PISM;
            //CounterStart = document.Counter.Start;
            //CounterFinish = document.Counter.Finish;
            //WeightNetto = document.Weight.Weight;
            //WeightBruto = document.Weight.Bruto;
            //SPU = document.SPU;
            BeamNumber = beamNumber;
        }
    }
}