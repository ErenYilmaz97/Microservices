using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Services.Catolog.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection serviceCollection, Type profile)
        {
            return serviceCollection.AddAutoMapper(profile);
        }
    }
}