using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using SignalR.DAL;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalR.Unity
{
    public class UnityConfiguration
    {
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IDataLayer, DataLayer>();
            return container;
        }

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            //DependencyResolver
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IDataLayer, DataLayer>();
            //container.RegisterType<IHubActivator, UnityHubActivator>(new ContainerControlledLifetimeManager());
            //container.RegisterType<IRepositoryUnityTestClass, RepositoryUnityTestClass>();
        }
    }
}