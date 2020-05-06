using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace TransactionTest
{
    public interface IDataLayer
    {
        public abstract List<User> GetData(int Id);

        public abstract List<User> UsingGetData(int Id);

        public abstract void UsingUpsertData(int Id, string Name, int Age);

        public abstract void UpsertData(int Id, string Name, int Age);
    }
}
