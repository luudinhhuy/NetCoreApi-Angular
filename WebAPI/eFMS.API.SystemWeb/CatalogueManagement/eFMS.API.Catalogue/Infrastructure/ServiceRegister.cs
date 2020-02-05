﻿using ITL.NetCore.Connection.EF;
using Microsoft.Extensions.DependencyInjection;
using eFMS.API.Catalogue.Service.Contexts;
using Microsoft.Extensions.Localization;
using LocalizationCultureCore.StringLocalizer;
using eFMS.API.Catalogue.DL.IService;
using eFMS.API.Catalogue.DL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using eFMS.IdentityServer.DL.UserManager;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using eFMS.API.Catalogue.Infrastructure.Filters;
using eFMS.API.Common;
using System.IO;
using System.Reflection;
using System;
using StackExchange.Redis;
using ITL.NetCore.Connection.Caching;
using eFMS.API.Catalogue.Service.Models;
using eFMS.API.Catalogue.Infrastructure.Common;
using eFMS.API.Catalogue.DL.Common;
using System.Security.Claims;
using IdentityModel;

namespace eFMS.API.Catalogue.Infrastructure
{
    public static class ServiceRegister
    {

        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IStringLocalizer, JsonStringLocalizer>();
            services.AddTransient<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddScoped(typeof(IContextBase<>), typeof(Base<>));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            
            services.AddSingleton<ICacheServiceBase<CatCountry>>(x =>
            new CacheServiceBase<CatCountry>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatCountry)));

            services.AddSingleton<ICacheServiceBase<CatArea>>(x =>
            new CacheServiceBase<CatArea>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatArea)));

            services.AddSingleton<ICacheServiceBase<CatCurrency>>(x =>
            new CacheServiceBase<CatCurrency>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatCurrency)));

            services.AddSingleton<ICacheServiceBase<CatChargeDefaultAccount>>(x =>
            new CacheServiceBase<CatChargeDefaultAccount>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatCharge)));

            services.AddSingleton<ICacheServiceBase<CatCharge>>(x =>
            new CacheServiceBase<CatCharge>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatCharge)));

            services.AddSingleton<ICacheServiceBase<CatPartner>>(x =>
            new CacheServiceBase<CatPartner>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatPartner)));

            services.AddSingleton<ICacheServiceBase<CatCommodityGroup>>(x =>
            new CacheServiceBase<CatCommodityGroup>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatCommodityGroup)));

            services.AddSingleton<ICacheServiceBase<CatCommodity>>(x =>
            new CacheServiceBase<CatCommodity>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatCommodity)));

            services.AddSingleton<ICacheServiceBase<CatPlace>>(x =>
            new CacheServiceBase<CatPlace>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatPlace)));

            services.AddSingleton<ICacheServiceBase<CatStage>>(x =>
            new CacheServiceBase<CatStage>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatStage)));

            services.AddSingleton<ICacheServiceBase<CatUnit>>(x =>
            new CacheServiceBase<CatUnit>(x.GetRequiredService<IConnectionMultiplexer>()
            , Enum.GetName(typeof(CacheEntity), CacheEntity.CatStage)));

            services.AddTransient<ICurrentUser, CurrentUser>();
            services.AddTransient<ICatBranchService, CatBranchService>();
            services.AddTransient<ICatPlaceService, CatPlaceService>();
            services.AddTransient<ICatCountryService, CatCountryService>();
            services.AddTransient<ICatStageService, CatStageService>();
            services.AddTransient<ICatAreaService, CatAreaService>();
            services.AddTransient<ICatCommodityGroupService, CatCommodityGroupService>();
            services.AddTransient<ICatCommodityService, CatCommodityService>();
            services.AddTransient<ICatPartnerService, CatPartnerService>();
            services.AddTransient<ICatPartnerGroupService, CatPartnerGroupService>();
            services.AddTransient<ICatUnitService, CatUnitService>();
            services.AddTransient<ICatChargeService, CatChargeService>();
            services.AddTransient<ICatChargeDefaultAccountService, CatChargeDefaultService>();
            services.AddTransient<ICatCurrencyService, CatCurrencyService>();
            services.AddTransient<ICatCurrencyExchangeService, CatCurrencyExchangeService>();
            services.AddTransient<ICatSaleManService, CatSalemanService>();

        }

        public static IServiceCollection AddCulture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJsonLocalization(opts => opts.ResourcesPath = configuration["LANGUAGE_PATH"]);
            //Multiple language setting
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("vi-VN")
            };
            var localizationOptions = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            localizationOptions.RequestCultureProviders = new[]
            {
                 new RouteDataRequestCultureProvider()
                 {
                     RouteDataStringKey = "lang",
                     Options = localizationOptions
                 }
            };

            services.AddSingleton(localizationOptions);
            return services;
        }

        public static IServiceCollection AddAuthorize(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Authentication:Authority"];
                options.RequireHttpsMetadata = bool.Parse(configuration["Authentication:RequireHttpsMetadata"]);
                options.Audience = configuration["Authentication:ApiName"];
                options.SaveToken = true;
                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = async context =>
                    {
                        try
                        {
                            String userID = context.Principal.FindFirstValue("id");
                            
                                List<Claim> lstClaim = new List<Claim>();
                                lstClaim.Add(new Claim(JwtClaimTypes.Role, "ABC"));
                                context.Principal.AddIdentity(new ClaimsIdentity(lstClaim, JwtBearerDefaults.AuthenticationScheme, "name", "role"));
                        }
                        catch { }
                    }
                };
            });
            //.AddIdentityServerAuthentication(options =>
            //{
            //    options.Authority = configuration["Authentication:Authority"];
            //    options.RequireHttpsMetadata = bool.Parse(configuration["Authentication:RequireHttpsMetadata"]);
            //    options.ApiName = configuration["Authentication:ApiName"];
            //    options.ApiSecret = configuration["Authentication:ApiSecret"];
            //});
            services.Configure<WebUrl>(option => {
                option.Url = configuration.GetSection("WebUrl").Value;
            });
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    //options.DescribeAllEnumsAsStrings();
                    var provider = services.BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (xmlPath != null)
                    {
                        options.IncludeXmlComments(xmlPath);
                    }

                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerDoc(
                            description.GroupName,
                            new Info()
                            {
                                Title = $"eFMS Catalogue API {description.ApiVersion}",
                                Version = description.ApiVersion.ToString(),
                                Description = "eFMS Catalogue API Document"
                            });
                    }
                    options.DocumentFilter<SwaggerAddEnumDescriptions>();

                    var security = new Dictionary<string, IEnumerable<string>>{
                        { "Bearer", new string[] { }},
                    };

                    options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });

                    options.OperationFilter<AuthorizeCheckOperationFilter>(); // Required to use access token
                });
            return services;
        }
        public static IServiceCollection AddConfigureSetting(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<Settings>(options =>
            {
                options.MongoConnection
                    = configuration.GetSection("ConnectionStrings:MongoConnection").Value;
                options.MongoDatabase
                    = configuration.GetSection("ConnectionStrings:Database").Value;
                options.RedisConnection
                    = configuration.GetSection("ConnectionStrings:Redis").Value;
                options.eFMSConnection
                    = configuration.GetSection("ConnectionStrings:eFMSConnection").Value;
            });
            return service;
        }
        public static IServiceCollection AddCrossOrigin(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .WithHeaders("accept", "content-type", "origin", "x-custom-header")
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
            return services;
        }
    }
}
