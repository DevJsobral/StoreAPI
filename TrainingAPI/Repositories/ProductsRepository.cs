using TrainingAPI.Context;
using TrainingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TrainingAPI.Repositories.Interfaces;

namespace TrainingAPI.Repositories;

public class ProductsRepository : Repository<Product>, IProductsRepository
{
    public ProductsRepository(TrainingAPIContext context) : base(context)
    {
        _context = context;
    }

    public IQueryable<Product> GetAllProducts()
    {
        return _context.Products.AsQueryable();
    }

}
