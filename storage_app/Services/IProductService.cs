﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using storage_app.Models;

namespace storage_app.Services
{
    internal interface IProductService
    {
        public Task<List<Product>> GetProducts();
        public Task<Product> GetProductById(int Id);
    }
}