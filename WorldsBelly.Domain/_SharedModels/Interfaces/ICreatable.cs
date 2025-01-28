using System;

namespace WorldsBelly.Domain.Interfaces
{
    public interface IDateCreated
    {
        DateTimeOffset DateCreated { get; set; }
    }

    public interface IUserCreatable : IDateCreated
    {
        string CreatedByUserId { get; set; }
    }
}
