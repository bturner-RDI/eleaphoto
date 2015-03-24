using AzureApp.Business.Services;
using AzureApp.Utilities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject.Modules;
using System.Reflection;
using AzureApp.Model;

namespace AzureApp.Business.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel(new ServiceModule());
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    ninjectKernel.Load(assembly);
                }
                catch (Exception)
                {
                    //Empty Catch used because ninject have problem
                    //with loading some of the Sitecore MVC assemblies.
                    // Method .ToString()
                }
            }
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return (controllerType == null) ? null : (IController)ninjectKernel.Get(controllerType);
        }
    }

    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IImageService>().To<ImageService>();
            Bind<IMapper>().To<Mapper>();
            Bind<IImageContext>().To<ImageContext>();
        }
    }

}