using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingAPI.Models;
using TrainingAPI.Repositories.Interfaces;

namespace TrainingAPI.Repositories;

public interface IOrdersRepository : IRepository<Order>
{ 
    Task<IEnumerable<Order>> GetAllOrders();
}

