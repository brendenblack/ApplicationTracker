using ApplicationTracker.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respawn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ApplicationTracker.Core.IntegrationTests
{
    public class TestBase
    {
        protected IConfigurationRoot _configuration;
        protected IServiceScopeFactory _scopeFactory;
        protected Checkpoint _checkpoint;

        public TestBase(ITestOutputHelper output = null)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", true, true)
               .AddEnvironmentVariables();

            _configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddCore(_configuration);

            //services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            //    w.EnvironmentName == "Development" &&
            //    w.ApplicationName == "Firewatch.WebUI"));

            if (output != null)
            {
                services.AddLogging(cfg =>
                {
                    cfg.AddProvider(new XUnitLoggerProvider(output));
                    cfg.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
                });
            }


            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };

            EnsureDatabase();
            ResetState().Wait();
        }

        protected void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.Migrate();
        }

        protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        protected async Task ResetState()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<TEntity> FindAsync<TEntity>(int id)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            

            return await context.FindAsync<TEntity>(id);
        }

        public async Task<TEntity> FindAsync<TEntity>(int id, string[] referenceIncludes = null, string[] collectionIncludes = null)
            where TEntity : class
        {

            referenceIncludes ??= new string[] { };
            collectionIncludes ??= new string[] { };

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var entity = await context.FindAsync<TEntity>(id);

            foreach (var include in referenceIncludes)
            {
                await context.Entry(entity).Reference(include).LoadAsync();
            }

            foreach (var include in collectionIncludes)
            {
                await context.Entry(entity).Collection(include).LoadAsync();
            }
            
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Gross hack workaround, try not to use it. This is included because the generic <see cref="FindAsync{TEntity}(int)"/> methods
        /// aren't able to return related entities
        /// /// </remarks>
        public ApplicationDbContext GetContext()
        {
            using var scope = _scopeFactory.CreateScope();

            return scope.ServiceProvider.GetService<ApplicationDbContext>();
        }

        public async Task<TEntity> FindAsync<TEntity>(string id)
        where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            return await context.FindAsync<TEntity>(id);
        }

        public async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public async Task<TKey> AddAsync<TEntity, TKey>(TEntity entity, PropertyInfo key)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();

            return (TKey)key.GetValue(entity);
        }

    }
}
