using Library.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests.Data
{
    public class DataContextTests
    {
        [Fact]
        public void TestName()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("TestDbContext")
                .Options;

            var config = new ConfigurationBuilder().Build();
            using var context = new DataContext(config);
            Assert.NotNull(context);
        }
    }
}