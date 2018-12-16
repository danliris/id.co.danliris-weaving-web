// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Manufactures.Domain.Orders.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manufactures.ViewModels.Manufacture
{
    public class CreateViewModel
    {
        [Display(Name = "Date")]
        [Required]
        public DateTimeOffset OrderDate { get; set; }

        [Display(Name = "Unit")]
        [Required]
        public int UnitDepartmentId { get; set; }

        [Display(Name = "Yarn Codes")]
        [Required]
        public List<string> YarnCodes { get; set; }

        [Display(Name = "Blended")]
        [Required]
        public List<float> Blended { get; set; }

        [Display(Name = "Machine")]
        [Required]
        public int MachineId { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        [Display(Name = "User")]
        [Required]
        public string UserId { get; set; }

        [Display(Name = "Construction")]
        public GoodsCompositionId CompositionId { get; set; }
    }
}