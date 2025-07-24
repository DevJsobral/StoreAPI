using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingAPI.Models;

namespace TrainingAPI.Repositories.Interfaces;

public interface IUnitOfWork
{
    ICategoriesRepository? CategoriesRepository { get; }
    IProductsRepository? ProductsRepository { get; }
    IOrdersRepository OrdersRepository { get; }

    async Task Commit()
    { }
}
