using Autofac;
using System.Reflection;
using WorldsBelly.API.Services;
using WorldsBelly.API.Services.Interfaces;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Repository;
using WorldsBelly.DataAccess.Repository.Interfaces;

namespace WorldsBelly.API.IoC
{
    public class APIAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(
                Assembly.GetAssembly(typeof(AppDbContext)),
                Assembly.GetAssembly(typeof(ICountryRepository)),
                Assembly.GetAssembly(typeof(CountryRepository)),
                Assembly.GetAssembly(typeof(ICountryService)),
                Assembly.GetAssembly(typeof(CountryService)))
            .AsImplementedInterfaces();
        }
    }
}
