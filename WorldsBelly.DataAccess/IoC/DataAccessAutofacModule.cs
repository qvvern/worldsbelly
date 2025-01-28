using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;
using System.Security.Principal;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Repository;
using WorldsBelly.DataAccess.Repository.Interfaces;
using WorldsBelly.DataAccess.Services;
using WorldsBelly.DataAccess.Services.Interfaces;

namespace WorldsBelly.DataAccess.IoC
{
    public class DataAccessAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(
                Assembly.GetAssembly(typeof(AppDbContext)),
                Assembly.GetAssembly(typeof(IRecipeRepository)),
                Assembly.GetAssembly(typeof(RecipeRepository)),
                Assembly.GetAssembly(typeof(IAzureAdB2CService)),
                Assembly.GetAssembly(typeof(AzureAdB2CService)))
                   .AsImplementedInterfaces();
                   //.InstancePerLifetimeScope();
        }
    }
}
