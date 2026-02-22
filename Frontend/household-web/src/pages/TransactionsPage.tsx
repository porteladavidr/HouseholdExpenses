import { useEffect, useMemo, useState } from 'react'
import { api } from '../api/endpoints'
import type { PersonListDto } from '../types/person'
import type { CategoryListDto } from '../types/category'
import type { TransactionListDto, TransactionType } from '../types/transaction'

export default function TransactionsPage() {
  const [people, setPeople] = useState<PersonListDto[]>([])
  const [categories, setCategories] = useState<CategoryListDto[]>([])
  const [items, setItems] = useState<TransactionListDto[]>([])
  const [error, setError] = useState<string | null>(null)

  // form
  const [description, setDescription] = useState('')
  const [value, setValue] = useState<number>(0)
  const [personId, setPersonId] = useState<number | ''>('')
  const [type, setType] = useState<TransactionType>('Despesa')
  const [categoryId, setCategoryId] = useState<number | ''>('')

  const selectedPerson = useMemo(
    () => people.find(p => p.id === personId),
    [people, personId]
  )

  useEffect(() => {
    if (selectedPerson && selectedPerson.age < 18 && type !== 'Despesa') {
      setType('Despesa')
    }
  }, [selectedPerson, type])

  const filteredCategories = useMemo(() => {
    if (!type) return categories
    return categories.filter(c => {
      if (type === 'Despesa') return c.purpose === 'Despesa' || c.purpose === 'Ambas'
      return c.purpose === 'Receita' || c.purpose === 'Ambas'
    })
  }, [categories, type])

  useEffect(() => {
    if (categoryId === '') return
    if (!filteredCategories.some(c => c.id === categoryId)) setCategoryId('')
  }, [filteredCategories, categoryId])

  async function loadAll() {
    setError(null)
    try {
      const [p, c, t] = await Promise.all([
        api.people.list(),
        api.categories.list(),
        api.transactions.list(),
      ])
      setPeople(p)
      setCategories(c)
      setItems(t)
    } catch (e: any) {
      setError(e.message)
    }
  }

  async function create() {
    setError(null)
    try {
      if (!personId || !categoryId) {
        setError('Selecione pessoa e categoria.')
        return
      }

      await api.transactions.create({
        description,
        value,
        type,
        personId,
        categoryId
      })

      setDescription('')
      setValue(0)
      setCategoryId('')
      await loadAll()
    } catch (e: any) {
      setError(e.message)
    }
  }

  useEffect(() => { loadAll() }, [])

  const canCreate =
    description.trim().length > 0 &&
    value > 0 &&
    personId !== '' &&
    categoryId !== '' &&
    (!selectedPerson || selectedPerson.age >= 18 || type === 'Despesa')

  return (
    <section className="page">
      <h2>Transações</h2>

      {error && <div className="alert">{error}</div>}

      <div className="card-pad" style={{ paddingTop: 0 }}>
        <div style={{ display: 'grid', gap: 10, maxWidth: 820 }}>
          <input
            className="input"
            placeholder="Descrição"
            value={description}
            onChange={e => setDescription(e.target.value)}
          />

          <div style={{ display: 'flex', gap: 10, flexWrap: 'wrap', alignItems: 'center' }}>
            <input
              className="input"
              placeholder="Valor"
              type="number"
              value={value}
              onChange={e => setValue(Number(e.target.value))}
              style={{ width: 160 }}
              min={0}
              step="0.01"
            />

            <select
              className="input"
              value={personId}
              onChange={e => setPersonId(e.target.value ? Number(e.target.value) : '')}
              style={{ minWidth: 230 }}
            >
              <option value="">Selecione a pessoa</option>
              {people.map(p => (
                <option key={p.id} value={p.id}>
                  {p.name} ({p.age})
                </option>
              ))}
            </select>

            <select
              className="input"
              value={type}
              onChange={e => setType(e.target.value as TransactionType)}
              disabled={!!selectedPerson && selectedPerson.age < 18}
              title={selectedPerson && selectedPerson.age < 18 ? 'Menor de idade: apenas despesa' : ''}
              style={{ width: 150 }}
            >
              <option value="Despesa">Despesa</option>
              <option value="Receita">Receita</option>
            </select>

            <select
              className="input"
              value={categoryId}
              onChange={e => setCategoryId(e.target.value ? Number(e.target.value) : '')}
              style={{ minWidth: 260 }}
            >
              <option value="">Selecione a categoria</option>
              {filteredCategories.map(c => (
                <option key={c.id} value={c.id}>
                  {c.description} ({c.purpose})
                </option>
              ))}
            </select>

            <button className="btn primary" onClick={create} disabled={!canCreate}>Adicionar</button>
            <button className="btn" onClick={loadAll}>Recarregar</button>
          </div>

          {selectedPerson && selectedPerson.age < 18 && (
            <div style={{ color: 'var(--muted)', fontSize: 12 }}>
              Menor de idade: apenas despesas (API valida e o tipo fica travado em Despesa).
            </div>
          )}
        </div>
      </div>

      <div className="card-pad" style={{ paddingTop: 0 }}>
        <table className="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Descrição</th>
            <th className="right">Valor</th>
            <th>Tipo</th>
            <th>Pessoa</th>
            <th>Categoria</th>
          </tr>
        </thead>
        <tbody>
          {items.map(t => (
            <tr key={t.id}>
              <td>{t.id}</td>
              <td>{t.description}</td>
              <td className="right">{t.value}</td>
              <td>{t.type}</td>
              <td>{t.personName}</td>
              <td>{t.categoryDescription}</td>
            </tr>
          ))}
          {items.length === 0 && (
            <tr><td colSpan={6}>Nenhuma transação cadastrada.</td></tr>
          )}
        </tbody>
        </table>
      </div>
    </section>
  )
}