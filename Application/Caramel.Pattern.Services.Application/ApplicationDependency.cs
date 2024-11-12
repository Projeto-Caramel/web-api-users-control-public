using Caramel.Pattern.Services.Application.Services.Adopters;
using Caramel.Pattern.Services.Application.Services.Bucket;
using Caramel.Pattern.Services.Application.Services.Partners;
using Caramel.Pattern.Services.Domain.Services.Adopters;
using Caramel.Pattern.Services.Domain.Services.Bucket;
using Caramel.Pattern.Services.Domain.Services.Partners;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Application
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationDependency
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IAdopterService, AdopterService>();
            services.AddScoped<IBucketService, BucketService>();

            return services;
        }
    }
}
