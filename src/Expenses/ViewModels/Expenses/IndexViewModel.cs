// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Expenses.ViewModels.Shared;
using System.Collections.Generic;

namespace Expenses.ViewModels.Expenses
{
    public class IndexViewModel
    {
        public IEnumerable<ExpenseViewModel> Expenses { get; set; }
    }
}