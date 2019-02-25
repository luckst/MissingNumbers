namespace PuntosColombia.MissingNumbers.API.DependencyInjection
{
    using AutoMapper;
    using PuntosColombia.MissingNumbers.Infrastructure.Data.DBFactory;
    using PuntosColombia.MissingNumbers.Infrastructure.Data.EntityFramework;
    using PuntosColombia.MissingNumbers.Infrastructure.Data.Repositories;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.Instrumentation.Logging;
    using PuntosColombia.MissingNumbers.Infrastructure.Framework.RepositoryPattern;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Serilog.Events;
    using Serilog.Sinks.MSSqlServer;
    using System.Collections.ObjectModel;
    using System.Data;

    public class NativeInjectorBootStrapper
    {
        /// <summary>
        /// Resolver la dependencia de los servicios
        /// </summary>
        /// <param name="services"></param>
        public void RegisterServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            // Build configuration
            /*var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false)
                .Build();*/

            services.AddSingleton(configuration);

            //Automapper
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            // Add logging
            #region log
            var columnOptions = new ColumnOptions
            {
                AdditionalDataColumns = new Collection<DataColumn>
               {
                    new DataColumn {DataType = typeof (int), ColumnName = "Priority"},
                    new DataColumn {DataType = typeof (string), ColumnName = "Title"},
                    new DataColumn {DataType = typeof (string), ColumnName = "MachineName"},
                    new DataColumn {DataType = typeof (string), ColumnName = "AppDomainName"},
                    new DataColumn {DataType = typeof (string), ColumnName = "ProcessID"},
                    new DataColumn {DataType = typeof (string), ColumnName = "ProcessName"},
               }
            };

            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Remove(StandardColumn.Properties);

            /*columnOptions.Store.Remove(StandardColumn.Message);
            columnOptions.Store.Remove(StandardColumn.Level);
            columnOptions.Store.Remove(StandardColumn.TimeStamp);
            columnOptions.Store.Remove(StandardColumn.Exception);
            columnOptions.Store.Remove(StandardColumn.LogEvent);*/

            /*columnOptions.MessageTemplate.ColumnName = "";
            columnOptions.Properties.ColumnName = "";*/
            columnOptions.Message.ColumnName = "Message";
            columnOptions.Level.ColumnName = "Severity";
            columnOptions.TimeStamp.ColumnName = "Timestamp";
            columnOptions.Exception.ColumnName = "FormattedMessage";
            columnOptions.LogEvent.ColumnName = "LogEvent";


            services.AddSingleton<Serilog.ILogger>
            (x => new LoggerConfiguration()
                 .MinimumLevel.Verbose()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                 .MinimumLevel.Override("System", LogEventLevel.Error)
                 .WriteTo.MSSqlServer(configuration["Serilog:ConnectionString"]
                 , configuration["Serilog:TableName"]
                 , LogEventLevel.Verbose
                 , columnOptions: columnOptions
                 //, schemaName: configuration["Serilog:Schema"]
                 )
                 .CreateLogger());

            //var file = File.CreateText("C:/Docs/Serilog.txt");
            //Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));
            #endregion


            // Application
            //services.AddScoped<ISecurityService, SecurityService>();

            //Domain
            //services.AddScoped<IChannelService, ChannelService>();

            //Repositories
            //services.AddScoped<IProductChannelRepository, ProductChannelRepository>();
            //services.AddScoped<IScheduledTaskHistoryRepository, ScheduledTaskHistoryRepository>();

            // Infrastructure

            services.AddScoped<ILoggerService, LoggerService>();

            // Infra - Data

            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<DbContext>(sp => sp.GetService<DBContext>());
            //services.AddScoped<DBCatalogContext>();

            services.AddDbContext<DBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                //options.UseOpenIddict();
            });

            //Auth0
            //services.AddSingleton<IAuthorizationHandler, HasChannelAuthorizationHandler>();

        }
    }
}
