using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SearchEnginesApp.Models;
using SearchEnginesApp.Services.Repository;
using SearchEnginesApp.Services.SearchEngine;
using SearchEnginesApp.Services.SearchEngine.Bing;
using SearchEnginesApp.Services.SearchEngine.Google;
using SearchEnginesApp.Services.SearchEngine.Yandex;
using SearchEnginesApp.Services.WebSearch;

namespace SearchEnginesApp
{
    public class Startup
    {
        readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SearchContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
            services.AddScoped<ISearchResultRepository, SearchResultRepository>();
            services.AddTransient<IWebSearchService, WebSearchService>();
            services.AddTransient<ISearchEngine, BingSearchEngine>();
            services.AddTransient<IWebDataSearch, WebDataSearch>();
            services.Configure<BingSearchOptions>(configuration.GetSection("BingSearchOptions"));
            services.AddTransient<ISearchEngine, GoogleSearchEngine>();
            services.AddTransient<ICseSearch, CseSearch>();
            services.Configure<GoogleSearchOptions>(configuration.GetSection("GoogleSearchOptions"));
            services.AddTransient<ISearchEngine, YandexSearchEngine>();
            services.AddTransient<IXDocumentLoader, XDocumentLoader>();
            services.Configure<YandexSearchOptions>(configuration.GetSection("YandexSearchOptions"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
