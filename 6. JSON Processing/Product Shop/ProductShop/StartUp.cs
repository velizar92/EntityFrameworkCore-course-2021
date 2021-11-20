using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {

        static IMapper mapper;
        public static void Main(string[] args)
        {

            var productShopContext = new ProductShopContext();
            productShopContext.Database.EnsureDeleted();
            productShopContext.Database.EnsureCreated();



            string inputJsonUsers = File.ReadAllText("../../../Datasets/users.json");
            var userResult = ImportUsers(productShopContext, inputJsonUsers);

            string inputJson = File.ReadAllText("../../../Datasets/products.json");
            var productResult = ImportProducts(productShopContext, inputJson);

            string inputJsonCategories = File.ReadAllText("../../../Datasets/categories.json");
            var categoryResult = ImportCategories(productShopContext, inputJsonCategories);

            string inputJsonCategoriesProducts = File.ReadAllText("../../../Datasets/categories-products.json");
            var categoryProductResult = ImportCategories(productShopContext, inputJsonCategoriesProducts);

            Console.WriteLine(userResult);
            Console.WriteLine(productResult);
            Console.WriteLine(categoryResult);
            Console.WriteLine(categoryProductResult);
        }



        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutomapper();

            IEnumerable<CategoryProductInputModel> dtoCategoriesProducts = JsonConvert
                .DeserializeObject<IEnumerable<CategoryProductInputModel>>(inputJson);

            var categoriesProducts = mapper.Map<IEnumerable<CategoryProduct>>(dtoCategoriesProducts);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {dtoCategoriesProducts.Count()}";
        }



        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            InitializeAutomapper();
           
            IEnumerable<CategoryInputModel> dtoCategories = JsonConvert
                .DeserializeObject<IEnumerable<CategoryInputModel>>(inputJson)
                .Where(x => x.Name != null)
                .ToList();
                
            var categories = mapper.Map<IEnumerable<Category>>(dtoCategories);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }



        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            InitializeAutomapper();
            IEnumerable<ProductInputModel> dtoProducts = JsonConvert.DeserializeObject<IEnumerable<ProductInputModel>>(inputJson);

            var products = mapper.Map<IEnumerable<Product>>(dtoProducts);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }



        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            InitializeAutomapper();
            IEnumerable<UserInputModel> dtoUsers = JsonConvert.DeserializeObject<IEnumerable<UserInputModel>>(inputJson);

            var users = mapper.Map<IEnumerable<User>>(dtoUsers);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }


        public static void InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            mapper = config.CreateMapper();
        }


    }
}