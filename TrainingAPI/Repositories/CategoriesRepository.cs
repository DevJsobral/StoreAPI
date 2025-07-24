using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingAPI.Context;
using TrainingAPI.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using TrainingAPI.Repositories.Interfaces;

namespace TrainingAPI.Repositories;

public class CategoriesRepository : Repository<Category>, ICategoriesRepository
{
    public CategoriesRepository(TrainingAPIContext context) : base(context)
    {
        _context = context;
    }
}
