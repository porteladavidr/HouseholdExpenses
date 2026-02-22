using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Household.Domain.Enums;

namespace Household.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    [Required, MaxLength(400)]
    public string Description { get; set; } = string.Empty;

    public CategoryPurpose Purpose { get; set; }
}
