using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace TransactionTest
{
    public class UserDbContext : DbContext 
    {
        //public UserDbContext(DbContextOptions<UserDbContext> options):base(options)
        //{
        //}
        public IDbContextTransaction SetTransaction()
        {
            return this.Database.BeginTransaction();
        }

        public virtual User SelectWithUpdLock(int Id)
        {
            return Users.FromSqlInterpolated($"select * from [dbo].[User] with (updlock)").Where(x => x.Id == Id).FirstOrDefault();

        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
        protected override void  OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-MNR5L7A\\SQLEXPRESS01;Initial Catalog=sample;Integrated Security=True;");
        }
    }
}
