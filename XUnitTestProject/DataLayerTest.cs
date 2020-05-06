using System;
using System.Collections.Generic;
using System.Text;
using TransactionTest;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage;

namespace XUnitTestProject
{
    public class DataLayerTest
    {
        [Fact]
        public void Test1()
        {
            //var loggerMock = new Mock<ILogger>();
            var dataEntity = new List<User>
            {
                new User{Id = 1, Name = "okamoto", Age = 30},
                new User{Id = 2, Name = "oka", Age = 20}
            }.AsQueryable();

            //DbSetmock

            var mockEntity = new Mock<DbSet<User>>();
            mockEntity.As<IQueryable<User>>().Setup(m => m.Provider).Returns(dataEntity.Provider);
            mockEntity.As<IQueryable<User>>().Setup(m => m.Expression).Returns(dataEntity.Expression);
            mockEntity.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(dataEntity.ElementType);
            mockEntity.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(dataEntity.GetEnumerator());

            //DbContextMock
            var dbContextMock = new Mock<UserDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(mockEntity.Object);
            var target = new DataLayer(dbContextMock.Object);
            var result = target.GetData(1);
           
            Assert.Equal<int>(2, result.Count);


        }
        [Fact]
        public void TestUpdate()
        {
            //DbContextMock
            var dbContextMock = new Mock<UserDbContext>();
            dbContextMock.Setup(x => x.SelectWithUpdLock(It.IsAny<int>())).Returns(new User { Id = 1, Name = "okamoto", Age = 30 });
            var target = new DataLayer(dbContextMock.Object);
            target.UpsertData(1, "sss", 20);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
            dbContextMock.Verify(m => m.Add(It.IsAny<User>()), Times.Never());
        }

        [Fact]
        public void TestInsert()
        {
            //DbContextMock
            var dbContextMock = new Mock<UserDbContext>();
            dbContextMock.Setup(x => x.SelectWithUpdLock(It.IsAny<int>())).Returns((User)null);
            var target = new DataLayer(dbContextMock.Object);
            target.UpsertData(1, "sss", 20);
            dbContextMock.Verify(m => m.SaveChanges(), Times.Once());
            dbContextMock.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
        }
    }
}
