export type TotalsByPersonItemDto = {
  personId: number
  name: string
  age: number
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export type TotalsSummaryDto = {
  totalReceitas: number
  totalDespesas: number
  saldo: number
}

export type TotalsByPersonReportDto = {
  itens: TotalsByPersonItemDto[]
  totalGeral: TotalsSummaryDto
}