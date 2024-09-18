// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.ActuatorApi.Models;

namespace Steeltoe.Samples.ActuatorApi.Data;

public sealed class WeatherContext(DbContextOptions<WeatherContext> options)
    : DbContext(options)
{
    public DbSet<WeatherForecast> Forecasts { get; set; }
}
