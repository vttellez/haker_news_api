using System;
using HackerNews.Core.Proxy.Serialization;
using HackerNews.Core.Proxy.Serialization.Json;
using HackerNews.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace HackerNews.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(options =>
            {
                options.AddPolicy("MyAllowSpecificOrigins",
                       builder =>
                       {
                           // prod application must restrict the origins ex builder.WithOrigins("http://domain.com")
                           builder.WithOrigins("*")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                       });

            });
            services.AddControllers();

            // For simplicity I will add services registration here. This can be extracted in a Middleware to handle common services registrationv
            RegisterServices(services);

        }

        /// <summary>
        /// Register services
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IObjectSerializer>(new JsonObjectSerializer(new JsonSerializerSettings()));
            services.AddHttpProxy<IHackerNewsProxy, HackerNewsProxy>("HackerNewsApiBaseUrl");
            services.AddScoped<IHackerNewsService, HackerNewsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("MyAllowSpecificOrigins");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}


public static class ConfigureServicesExtension
{

    public static IHttpClientBuilder AddHttpProxy<TClient, TImplementation>(
           this IServiceCollection services,
           string baseAddressKey = null)
           where TClient : class
           where TImplementation : class, TClient
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IObjectSerializer>(new JsonObjectSerializer(new JsonSerializerSettings()));

        var builder = services
            .AddHttpClient<TClient, TImplementation>()
            .ConfigureHttpClient((provider, client) =>
            {
                if (!string.IsNullOrWhiteSpace(baseAddressKey))
                {
                    IConfiguration config = provider.GetService<IConfiguration>();

                    string baseAddress = GetBaseAddress(baseAddressKey, config);

                    if (string.IsNullOrWhiteSpace(baseAddress))
                    {
                        throw new ArgumentNullException(baseAddressKey);
                    }

                    client.BaseAddress = new Uri(baseAddress);
                }
            });
        return builder;
    }

    private static string GetBaseAddress(string baseAddressKey, IConfiguration config)
    {
        return config.GetSection("AppSettings").GetValue<string>(baseAddressKey);
    }
}
