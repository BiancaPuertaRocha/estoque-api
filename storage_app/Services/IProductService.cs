﻿using System.Collections.Generic;
using System.Threading.Tasks;

using storage_app.Models;

namespace storage_app.Services
{
    internal interface IProductService
    {
        public Task<List<Product>> GetProducts();
        public Task<List<Product>> GetProductsFiltered(int? id = null, string? description = null, string? category = null, int? quantity = null);
        public Task<Product?> GetProductById(int Id);
        public Task<Product?> InsertProduct(Product product);
        public Task<bool> DeleteProduct(int Id);
        public Task<Product?> UpdateProduct(int Id, Product product);
        public Task<Product?> SellProduct(int Id, int quantity, Product product);

    }
}
