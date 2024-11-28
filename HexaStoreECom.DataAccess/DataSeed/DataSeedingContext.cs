using HexaStoreECom.Entities.JsonModels;
using HexaStoreECom.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HexaStoreECom.DataAccess.DataSeed
{
    public static class DataSeedingContext
    {
        //steps
        // 1. read data from files
        // 2. convert data into string 
        // 3. add data into database
        public static async Task CategoryDataSeed(ApplicationDbContext dbContext, string filename = "ProductCategories.json")
        {
            if (dbContext.Categories.Any())
                return;
             await DataSeeding<Category>(dbContext, filename);
        }
        public static async Task ProductsDataSeed(ApplicationDbContext dbContext, string filename = "Products.json")
        {
            string basePath = "../HexaStoreECom.DataAccess/JsonFiles/";
            var textData = File.ReadAllText($"{basePath}{filename}");
            var jsonData = JsonSerializer.Deserialize<List<ProductJsonModel>>(textData);

            if (dbContext.Products.Any())
                return;
            var Categories = dbContext.Categories.ToList();
            var products = jsonData?.Select(product => 
            {
                var category = Categories.SingleOrDefault(C => C.Name == product.category);
                return new Product
                {
                    Name = product.title,
                    CategoryId = category?.Id ?? 0,
                    Description = product.description,
                    Img = product.image,
                    Price = product.price
                };
            });
            foreach (var item in products)
            {
                dbContext.Products.Add(item);
            }
            await dbContext.SaveChangesAsync();
        }
        public static async Task  DataSeeding<T> (ApplicationDbContext dbContext,string filename) where T : class
        {
            string basePath = "../HexaStoreECom.DataAccess/JsonFiles/";
            var textData= File.ReadAllText($"{basePath}{filename}");
            var jsonData = JsonSerializer.Deserialize<List<T>>(textData);
            if(jsonData?.Count>0)
            {
                foreach(var item in jsonData)
                {
                    await dbContext.Set<T>().AddAsync(item);
                }
                await dbContext.SaveChangesAsync();
            }

        }
    }
}
