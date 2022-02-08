using Autofac;
using AutoMapper;
using StreamFile.Mapper;

namespace StreamFile.WebApi.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var azureCon = SystemHelper.AzureServiceBus;
            //builder.Register(c => new AzureServiceBus.AzureServiceBusService(connectionString: azureCon)).As<AzureServiceBus.IQueueService>().SingleInstance();
            builder.Register(c =>
                    new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile(new DocumentsStoreProfile());
                    }).CreateMapper())
                .SingleInstance();
            RegisterQueue(builder);
        }

        private static void RegisterQueue(ContainerBuilder builder)
        {
            //builder.Register(c => new FundsTransferQueueHandler(c.Resolve<IServiceProvider>())).InstancePerDependency();

        }
    }
}