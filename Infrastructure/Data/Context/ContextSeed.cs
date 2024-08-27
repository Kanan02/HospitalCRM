using Domain.Entities;
using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context
{
    public class ContextSeed
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                  new Role { Id = Guid.NewGuid(), Name = "superadmin" },
                  new Role { Id = Guid.NewGuid(), Name = "admin" },
                  new Role { Id = Guid.NewGuid(), Name = "user" },
                  new Role { Id = Guid.NewGuid(), Name = "doctor" }
              );

            builder.Entity<Module>().HasData(
                  new Module { Id = Guid.NewGuid(), Name = "Queue Module", Description = "Module for queue managing." }
                );
        }
    }
}
