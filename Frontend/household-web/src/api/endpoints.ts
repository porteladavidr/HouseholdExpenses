import { http } from './http'
import type { PersonCreateDto, PersonListDto, PersonUpdateDto } from '../types/person'
import type { CategoryCreateDto, CategoryListDto } from '../types/category'
import type { TransactionCreateDto, TransactionListDto } from '../types/transaction'
import type { TotalsByPersonReportDto } from '../types/report'

export const api = {
  people: {
    list: () => http<PersonListDto[]>('/api/pessoas'),
    create: (dto: PersonCreateDto) =>
      http<PersonListDto>('/api/pessoas', { method: 'POST', body: JSON.stringify(dto) }),
    update: (id: number, dto: PersonUpdateDto) =>
      http<void>(`/api/pessoas/${id}`, { method: 'PUT', body: JSON.stringify(dto) }),
    remove: (id: number) =>
      http<void>(`/api/pessoas/${id}`, { method: 'DELETE' }),
  },

  categories: {
    list: () => http<CategoryListDto[]>('/api/categorias'),
    create: (dto: CategoryCreateDto) =>
      http<CategoryListDto>('/api/categorias', { method: 'POST', body: JSON.stringify(dto) }),
  },

  transactions: {
    list: () => http<TransactionListDto[]>('/api/transacoes'),
    create: (dto: TransactionCreateDto) =>
      http<{ id: number }>('/api/transacoes', { method: 'POST', body: JSON.stringify(dto) }),
  },

  reports: {
    totalsByPerson: () => http<TotalsByPersonReportDto>('/api/relatorios/totais-por-pessoa'),
  }
}