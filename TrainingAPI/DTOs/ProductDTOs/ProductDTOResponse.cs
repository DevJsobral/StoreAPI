using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainingAPI.DTOs;

public class ProductDTOResponse
{
    public int ProductId { get; set; }
    
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? CategoryId { get; set; }

    public int Stock { get; set; }

    public string? ImageURL { get; set; }

    public DateTime RegisterDate { get; set; }
}
