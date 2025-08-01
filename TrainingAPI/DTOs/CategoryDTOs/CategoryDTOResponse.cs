using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingAPI.DTOs.Mapping;

public class CategoryDTOResponse
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public string? ImageURL { get; set; }
}
