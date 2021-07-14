using Books.DataServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Books.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
            });

            builder.Services.AddHttpClient<IBookDataService, BookDataService>(client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration["WebApis:Books"]);
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", builder.Configuration["WebApis:ApimSubscriptionKey"]);
                }
            )
            .AddHttpMessageHandler(sp =>
                {
                    var provider = sp.GetRequiredService<IAccessTokenProvider>();
                    var naviManager = sp.GetRequiredService<NavigationManager>();

                    var handler = new AuthorizationMessageHandler(provider, naviManager);
                    handler.ConfigureHandler(authorizedUrls: new[]
                        {
                            builder.Configuration["WebApis:Books"]
                        }
                    );

                    return handler;
                }
            );

            await builder.Build().RunAsync();
        }
    }
}
