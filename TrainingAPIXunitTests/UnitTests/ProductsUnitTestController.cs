using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainingAPI.Context;
using TrainingAPI.DTOs.Mapping;
using TrainingAPI.Repositories;
using TrainingAPI.Repositories.Interfaces;

namespace TrainingAPIXunitTests;

public class ProductsUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;
    public static DbContextOptions<TrainingAPIContext> dbContextOptions { get; }

    public static string connectionString = "Server=localhost;DataBase=trainingapidb;Uid=root;Pwd=root";

    static ProductsUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<TrainingAPIContext>()
        .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        .Options;
    }

    public ProductsUnitTestController()
    {
        var config = new MapperConfiguration(config =>
        {
            config.AddProfile(new DTOMappingProfile());
        });

        mapper = config.CreateMapper();

        var context = new TrainingAPIContext(dbContextOptions);
        repository = new UnitOfWork(context);
    }
}
