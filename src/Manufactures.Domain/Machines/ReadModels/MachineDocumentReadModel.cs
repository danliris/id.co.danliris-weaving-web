﻿using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Machines.ReadModels
{
    public class MachineDocumentReadModel : ReadModelBase
    {
        public MachineDocumentReadModel(Guid identity) : base(identity) { }
        
        public string MachineNumber { get; internal set; }
        public string Unit { get; internal set; }
        public string MachineType { get; internal set; }
        public string Location { get; internal set; }
    }
}