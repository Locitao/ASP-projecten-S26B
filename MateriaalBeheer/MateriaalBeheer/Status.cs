﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaterialRenting
{
    /// <summary>
    /// Stores the current status off a Material: Undefined, Reserved or Free
    /// </summary>
    public enum Status
    {
        Undefined,
        Reserved,
        Free
    }
}