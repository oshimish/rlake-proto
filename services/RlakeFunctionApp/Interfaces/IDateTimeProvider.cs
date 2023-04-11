﻿using System;

namespace RlakeFunctionApp.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
