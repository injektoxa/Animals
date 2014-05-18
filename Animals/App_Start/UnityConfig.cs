using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Animals.Repository;
using Animals.Models;

namespace Animals
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IRepository<Owner>, SQLRepository<Owner>>();
            container.RegisterType<IRepository<Pet>, SQLRepository<Pet>>();
            container.RegisterType<IRepository<Doctor>, SQLRepository<Doctor>>();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}