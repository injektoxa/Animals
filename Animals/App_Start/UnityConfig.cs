using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Animals.Repository;
using Animals.Models;
using Animals.Controllers;

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
            container.RegisterType<IRepository<Anamne>, SQLRepository<Anamne>>();
            container.RegisterType<IRepository<Diagnostic>, SQLRepository<Diagnostic>>();
            container.RegisterType<IRepository<Surgical_treatment>, SQLRepository<Surgical_treatment>>();
            container.RegisterType<IRepository<Treatment>, SQLRepository<Treatment>>();
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterInstance(DbContext.Instance);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}