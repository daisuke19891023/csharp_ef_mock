using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;

namespace TransactionTest
{
    public class DataLayer : IDataLayer
    {
        private DbContext _dbContext;
        public DataLayer(DbContext dbContext)

        {
            _dbContext = dbContext;
        }
        public List<User> UsingGetData(int Id) { 
            using(var tran = _dbContext.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                return GetData(Id);
            }
                }
        public List<User> GetData(int Id)
        {
            var userEntities = (UserDbContext)_dbContext;

                var items = userEntities.Users.ToList();
                return items;
        }

        public void UsingUpsertData(int Id, string Name, int Age)
        {
            using (var tran = _dbContext.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                UpsertData(Id, Name,Age);
                tran.Commit();
            }

        }
        public void UpsertData(int Id, string Name, int Age)
        {
            var userEntities = (UserDbContext)_dbContext;

            var target = userEntities.SelectWithUpdLock(Id);
            if (target == null)
            {
                var user = new User { Id = Id, Name = Name, Age = Age };
                userEntities.Add(user);
            }
            else
            {
                target.Name = Name;
                target.Age = Age;
            }

            Thread.Sleep(10000);
            _dbContext.SaveChanges();

        }
    }
}
