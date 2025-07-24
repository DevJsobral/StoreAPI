using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingAPI.Models;
using TrainingAPI.Repositories.Interfaces;

namespace TrainingAPI.Repositories;

public interface IProductsRepository : IRepository<Product>
{
    public IQueryable<Product> GetAllProducts();
}
