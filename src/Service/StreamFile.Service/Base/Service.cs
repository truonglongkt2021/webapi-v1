using StreamFile.Contract.Repository.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace StreamFile.Service.Base
{
    public abstract class Service
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected Service(IServiceProvider serviceProvider)
        {
            UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }
    }
}
