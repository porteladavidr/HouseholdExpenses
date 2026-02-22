export type CategoryPurpose = 'Despesa' | 'Receita' | 'Ambas'

export type CategoryListDto = {
  id: number
  description: string
  purpose: CategoryPurpose
}

export type CategoryCreateDto = {
  description: string
  purpose: CategoryPurpose
}