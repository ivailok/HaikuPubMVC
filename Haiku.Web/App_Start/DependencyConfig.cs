using Autofac;
using Autofac.Integration.Mvc;
using Haiku.Data;
using Haiku.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Haiku.Web
{
    public class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();
            RegisterData(builder);
            RegisterServices(builder);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterData(ContainerBuilder builder)
        {
            builder.RegisterType<HaikuContext>().AsSelf().As<IDbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(DbAsyncRepository<>)).As(typeof(IAsyncRepository<>)).InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UsersService>().As<IUsersService>().InstancePerRequest();
            builder.RegisterType<HaikusService>().As<IHaikusService>().InstancePerRequest();
            builder.RegisterType<ReportsService>().As<IReportsService>().InstancePerRequest();
        }
    }
}