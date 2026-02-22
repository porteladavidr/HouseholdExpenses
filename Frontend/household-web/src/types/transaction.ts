export type TransactionType = 'Despesa' | 'Receita'

export type TransactionCreateDto = {
  description: string
  value: number
  type: TransactionType
  categoryId: number
  personId: number
}

export type TransactionListDto = {
  id: number
  description: string
  value: number
  type: TransactionType
  categoryId: number
  categoryDescription: string
  personId: number
  personName: string
}