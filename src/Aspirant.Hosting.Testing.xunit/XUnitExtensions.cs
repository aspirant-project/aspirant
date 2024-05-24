// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Aspirant.Hosting.Testing;

/// <summary>
/// Extension methods for configuring xUnit output.
/// </summary>
public static partial class DistributedApplicationTestFactory
{
    /// <summary>
    /// Writes <see cref="ILogger"/> messages and resource logs to the provided <see cref="ITestOutputHelper"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="testOutputHelper">The output.</param>
    /// <returns>The builder.</returns>
    public static IDistributedApplicationTestingBuilder WriteOutputTo(this IDistributedApplicationTestingBuilder builder, ITestOutputHelper testOutputHelper)
    {
        // Enable the core ILogger and resource output capturing
        builder.WriteOutputTo(new XUnitTextWriter(testOutputHelper));

        // Enable ILogger going to xUnit output
        builder.Services.AddSingleton(testOutputHelper);
        builder.Services.AddSingleton<ILoggerProvider, XUnitLoggerProvider>();

        return builder;
    }
}
