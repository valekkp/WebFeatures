﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WebFeatures.DatabaseInitializer.Core;

namespace WebFeatures.DatabaseInitializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new CompositionRoot();
            var scripts = root.Services.GetRequiredService<ScriptsRunner>();
            var logger = root.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                scripts.Run();

                logger.LogInformation("Finished successfully");

            }
            catch (Exception e)
            {
                logger.LogError(e, "Finished with an exception");
            }
        }
    }
}
