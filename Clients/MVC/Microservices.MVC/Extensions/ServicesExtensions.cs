using System;
using Microservices.MVC.Handler;
using Microservices.MVC.Models;
using Microservices.MVC.Services.Implementations;
using Microservices.MVC.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.MVC.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddHttpClientServices(this IServiceCollection services, IConfiguration Configuration)
        {
            var servicesApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();


            services.AddHttpClient<IClientCredentialsTokenService, ClientCredentialsTokenService>();
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(servicesApiSettings.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                opt.BaseAddress =
                    new Uri($"{servicesApiSettings.GatewayBaseUri}/{servicesApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
            {
                opt.BaseAddress =
                    new Uri($"{servicesApiSettings.GatewayBaseUri}/{servicesApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                opt.BaseAddress =
                    new Uri($"{servicesApiSettings.GatewayBaseUri}/{servicesApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IDiscountService, DiscountService>(opt =>
            {
                opt.BaseAddress =
                    new Uri($"{servicesApiSettings.GatewayBaseUri}/{servicesApiSettings.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IPaymentService, PaymentService>(opt =>
            {
                opt.BaseAddress =
                    new Uri($"{servicesApiSettings.GatewayBaseUri}/{servicesApiSettings.Payment.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IOrderService, OrderService>(opt =>
            {
                opt.BaseAddress =
                    new Uri($"{servicesApiSettings.GatewayBaseUri}/{servicesApiSettings.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }
    }
}