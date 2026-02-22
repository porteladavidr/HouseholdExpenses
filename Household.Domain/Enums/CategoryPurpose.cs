using System;
using System.Collections.Generic;
using System.Text;
namespace Household.Domain.Enums;

/// <summary>
/// Define para que a categoria pode ser usada.
/// A regra de negócio vai impedir usar categoria de Receita em transação de Despesa e vice-versa,
/// exceto quando Purpose = Ambas.
/// </summary>
public enum CategoryPurpose
{
    Despesa = 1,
    Receita = 2,
    Ambas = 3
}