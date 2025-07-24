using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingAPI.Context;
using TrainingAPI.Repositories.Interfaces;

namespace TrainingAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private TrainingAPIContext _context;
    private ICategoriesRepository _categoriesRepo;
    private IProductsRepository _productsRepo;
    private IOrdersRepository _ordersRepo;

    public UnitOfWork(TrainingAPIContext context)
    {
        _context = context;
    }

    public ICategoriesRepository CategoriesRepository
    {
        get
        {
            return _categoriesRepo = _categoriesRepo ?? new CategoriesRepository(_context);
        }
    }

    public IProductsRepository ProductsRepository
    {
        get
        {
            return _productsRepo = _productsRepo ?? new ProductsRepository(_context);
        }
    }
    
    public IOrdersRepository OrdersRepository
    {
        get
        {
            return _ordersRepo = _ordersRepo ?? new OrdersRepository(_context);
        }
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
