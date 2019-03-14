using CryptoHelper;
using Electrum.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Electrum.Data.Database
{
    public static class DataContextExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<User>().HasData(new User
            {
                Id = Guid.Parse("DA305776-4AD7-46A7-92AB-C2995989ECB2"),
                Created = DateTime.UtcNow,
                Email = "falken.ua@gmail.com",
                FirstName = "Valentyn",
                LastName = "Stepanov",
                Password = Crypto.HashPassword("password")
            });

            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.Parse("F4EB3E69-072B-4C7B-A008-F3399D658F46"),
                    Name = "Admin",
                    Created = DateTime.UtcNow,
                },
                new Role
                {
                    Id = Guid.Parse("930B8C06-DDC0-403A-BEB5-B720F382B723"),
                    Name = "User",
                    Created = DateTime.UtcNow
                });

            builder.Entity<Activity>().HasData(
                new Activity
                {
                    Id = Guid.Parse("43250236-B73F-4FDF-AC3D-A47B826F66FE"),
                    Name = "Jogging"
                },
                new Activity
                {
                    Id = Guid.Parse("74AC12FF-D1C8-4184-88C0-879DBA3644BB"),
                    Name = "Walking"
                },
                new Activity
                {
                    Id = Guid.Parse("5FD68730-F4B1-42D1-8E28-4DF684D4C1EE"),
                    Name = "Upstairs"
                },
                new Activity
                {
                    Id = Guid.Parse("78496D4A-51E4-412C-B5A6-73A45140FEBE"),
                    Name = "Downstairs"
                },
                new Activity
                {
                    Id = Guid.Parse("5DBEEAB3-91D5-4C62-8FD3-9CF5829141F1"),
                    Name = "Sitting"
                },
                new Activity
                {
                    Id = Guid.Parse("CAA42D3C-23C1-42E6-A339-3042FA63EA2E"),
                    Name = "Standing"
                });
        }
    }
}
