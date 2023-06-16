using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetProductsReturnsAllProductsAsync()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProductsAsync))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var productos = await productsProvider.GetProductsAsync();

            Assert.True(productos.IsSuccess);
            Assert.True(productos.Products.Any());
            Assert.Null(productos.ErrorMessage);
        }
        [Fact]
        public async Task GetProductsReturnsAllProductsAsyncUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductsReturnsAllProductsAsyncUsingValidId))
                .Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);
            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var producto = await productsProvider.GetProductsAsync(100);

            Assert.True(producto.IsSuccess);
            Assert.NotNull(producto.Product);
            Assert.True(producto.Product.Id == 100);
            Assert.Null(producto.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 100; i < 115; i++) {
                dbContext.Products.Add(
                    new Product() { 
                        Id = i,
                        Name = Guid.NewGuid().ToString(),
                        Inventory = i + 10,
                        Price = (decimal) (i * 3.14)
                    }
                );
            }
            dbContext.SaveChanges();
        }
    }
}
