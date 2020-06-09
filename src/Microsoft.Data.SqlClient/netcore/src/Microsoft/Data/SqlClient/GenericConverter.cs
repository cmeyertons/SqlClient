﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Data.SqlClient
{
    internal static class GenericConverter
    {
        // This leverages the same assumptions in SqlBuffer that the JIT will optimize out the boxing / unboxing when TIn == TOut
        public static TOut Convert<TIn, TOut>(TIn value) => (TOut)(object)value;
    }
}
