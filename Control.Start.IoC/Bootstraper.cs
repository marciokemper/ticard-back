﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.IoC
{
    public static class Bootstraper
    {
        public static void Configure(IServiceCollection services)
        {

            Configure(services, Data.IoC.Module.GetTypes());
            Configure(services, DomainServices.IoC.Module.GetTypes());
            Configure(services, AppServices.IoC.Module.GetTypes());

        }

        private static void Configure(IServiceCollection services, Dictionary<Type, Type> types)
        {
            foreach (var type in types)
                services.AddScoped(type.Key, type.Value);
        }

    }
}
