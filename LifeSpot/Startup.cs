using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LifeSpot
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            string footerHtml = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "footer.html"));
            string sideBarHtml = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "sideBar.html"));

            Dictionary<string, string> replaceItems = new Dictionary<string, string>()
            {
                { "<!--SIDEBAR-->", sideBarHtml },
                { "<!--FOOTER-->", footerHtml },
            };

            app.UseEndpoints(endpoints =>
            {
                AddMapGet(endpoints, "/", Path.Combine(Directory.GetCurrentDirectory(), "Views", "index.html"), replaceItems);
                AddMapGet(endpoints, "/testing", Path.Combine(Directory.GetCurrentDirectory(), "Views", "testing.html"), replaceItems);
                AddMapGet(endpoints, "/about", Path.Combine(Directory.GetCurrentDirectory(), "Views", "about.html"), replaceItems);
                AddMapGet(endpoints, "/Static/CSS/index.css", Path.Combine(Directory.GetCurrentDirectory(), "Static", "CSS", "index.css"));
                AddMapGet(endpoints, "/Static/JS/index.js", Path.Combine(Directory.GetCurrentDirectory(), "Static", "JS", "index.js"));
                AddMapGet(endpoints, "/Static/JS/testing.js", Path.Combine(Directory.GetCurrentDirectory(), "Static", "JS", "testing.js"));
                AddMapGet(endpoints, "/Static/JS/filterContent.js", Path.Combine(Directory.GetCurrentDirectory(), "Static", "JS", "filterContent.js"));
                AddMapGet(endpoints, "/Static/JS/about.js", Path.Combine(Directory.GetCurrentDirectory(), "Static", "JS", "about.js"));
            });

        }

        /// <summary>
        /// Метод для добавления MapGet'ов
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="patternt"></param>
        /// <param name="filePath"></param>
        /// <param name="replaceItems"></param>
        private async void AddMapGet(IEndpointRouteBuilder endpoint, string patternt, string filePath, Dictionary<string, string> replaceItems = null)
        {
            var html = new StringBuilder(await File.ReadAllTextAsync(filePath));
            if (replaceItems != null)
            {
                foreach (var element in replaceItems)
                {
                    html = html.Replace(element.Key, element.Value);
                }
            }
            endpoint.MapGet(patternt, async context =>
            {
                await context.Response.WriteAsync(html.ToString());
            });
        }
    }
}