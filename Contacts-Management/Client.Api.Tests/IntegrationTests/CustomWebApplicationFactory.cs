using Clients.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Api.Tests.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private string _inMemoryDatabaseName { get; set; }

        public CustomWebApplicationFactory(string inMemoryDatabaseName)
            : base()
        {
            _inMemoryDatabaseName = inMemoryDatabaseName;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var realDbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ClientsApiDbContext>));

                services.Remove(realDbContextDescriptor);

                services.AddDbContext<ClientsApiDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_inMemoryDatabaseName);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ClientsApiDbContext>();
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
