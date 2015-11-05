using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Web.Mappings;

namespace BizOneShot.Light.Web.App_Start
{
    public static class AutofacModuleConfig
    {
        public static void Run()
        {
            SetAutofacContainer();

            //AutoMapper 설정
            AutoMapperConfiguration.Configure();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
                //.InstancePerLifetimeScope();
                .InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>()
                //.InstancePerLifetimeScope();
                .InstancePerRequest();

            builder.RegisterType<DareUnitOfWork>().As<IDareUnitOfWork>()
                //.InstancePerLifetimeScope();
                .InstancePerRequest();

            builder.RegisterType<DareDbFactory>().As<IDareDbFactory>()
                //.InstancePerLifetimeScope();
                .InstancePerRequest();

            // Repositories
            //builder.RegisterAssemblyTypes(typeof(ScFaqRepository).Assembly)
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.Load("BizOneShot.Light.Dao"))
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            //Services
            //builder.RegisterAssemblyTypes(typeof(ScFaqService).Assembly)
            //   .Where(t => t.Name.EndsWith("Service"))
            //   .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.Load("BizOneShot.Light.Services"))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}