using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;
using TrainingAPI.Context;
using TrainingAPI.Models;
using TrainingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TrainingAPI.Repositories;

public class OrdersRepository : Repository<Order>, IOrdersRepository
{
    public OrdersRepository(TrainingAPIContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllOrders()
    {
        return await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .ToListAsync();
    }

}
