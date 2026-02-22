using System;
using System.Collections.Generic;
using System.Text;
using Household.Domain.Enums;

namespace Household.Application.DTOs.Categories;

public record CategoryListDto(int Id, string Description, CategoryPurpose Purpose);
