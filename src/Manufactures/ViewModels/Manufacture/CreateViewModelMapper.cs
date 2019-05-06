// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using System;

namespace Manufactures.ViewModels.Manufacture
{
    public class CreateViewModelMapper
    {
        public ManufactureOrder Map(CreateViewModel createViewModel, string currentUser)
        {
            return new ManufactureOrder(id: Guid.NewGuid(),
                    orderDate: createViewModel.OrderDate.Date,
                    unitId: new UnitDepartmentId(createViewModel.UnitDepartmentId),
                    yarnCodes: new YarnCodes(createViewModel.YarnCodes),
                    compositionId: createViewModel.CompositionId,
                    blended: new Blended(createViewModel.Blended),
                    machineId: new MachineIdValueObject(createViewModel.MachineId),
                    userId: currentUser);
        }
    }
}