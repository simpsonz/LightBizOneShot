﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

using Autofac;
using Autofac.Integration.Mvc;

using BizOneShot.Light.Dao.Infrastructure;
using BizOneShot.Light.Web.Mappings;
using BizOneShot.Light.Services;

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
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            // Repositories
            //builder.RegisterAssemblyTypes(typeof(ScCompInfoRepository).Assembly)
            //    .Where(t => t.Name.EndsWith("Repository"))
            //    .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.Load("BizOneShot.Light.Dao"))
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Services
            builder.RegisterAssemblyTypes(typeof(ScFaqService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.Load("BizOneShot.Light.Services"))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}