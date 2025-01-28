using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WorldsBelly.DataAccess.Contexts;
using WorldsBelly.DataAccess.Entities;
using WorldsBelly.DataAccess.Entities.Filters;

namespace WorldsBelly.DataAccess.Utilities.Extensions
{
    public static class DbContextExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> items, int page, int pageSize)
        {
            return items.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
