using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Household.Application.DTOs.People;

public record PersonCreateDto(
    [Required, MaxLength(200)] string Name,
    [Range(0, 130)] int Age
);
