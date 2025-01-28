using Autofac;
using System.Reflection;
using WorldsBelly.Puppeteers.Service;
using WorldsBelly.Puppeteers.Service.Interfaces;

namespace WorldsBelly.Puppeteers.IoC
{
    public class PuppeteersAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(
                Assembly.GetAssembly(typeof(IPuppeteerService)),
                Assembly.GetAssembly(typeof(PuppeteerService)))
            .AsImplementedInterfaces();
        }
    }
}
