using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Household.Domain.Enums;

namespace Household.Application.DTOs.Categories;

public record CategoryCreateDto(
    [Required, MaxLength(400)] string Description,
    CategoryPurpose Purpose
);