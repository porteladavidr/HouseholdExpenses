import { useEffect, useState } from 'react'
import { api } from '../api/endpoints'
import type { CategoryListDto, CategoryPurpose } from '../types/category'

export default function CategoriesPage() {
  const [items, setItems] = useState<CategoryListDto[]>([])
  const [description, setDescription] = useState('')
  const [purpose, setPurpose] = useState<CategoryPurpose>('Despesa')
  const [error, setError] = useState<string | null>(null)

  async function load() {
    setError(null)
    try {
      setItems(await api.categories.list())
    } catch (e: any) {
      setError(e.message)
    }
  }

  async function create() {
    setError(null)
    try {
      await api.categories.create({ description, purpose })
      setDescription('')
      setPurpose('Despesa')
      await load()
    } catch (e: any) {
      setError(e.message)
    }
  }

  useEffect(() => { load() }, [])

  return (
    <section className="page">
      <h2>Categorias</h2>

      {error && <div className="alert">{error}</div>}

      <div className="toolbar">
        <div style={{ display: 'flex', gap: 10, flexWrap: 'wrap', alignItems: 'center', width: '100%' }}>
          <input
            className="input"
            placeholder="Descrição"
            value={description}
            onChange={e => setDescription(e.target.value)}
            style={{ minWidth: 280 }}
          />

          <select className="input" value={purpose} onChange={e => setPurpose(e.target.value as CategoryPurpose)} style={{ width: 180 }}>
            <option value="Despesa">Despesa</option>
            <option value="Receita">Receita</option>
            <option value="Ambas">Ambas</option>
          </select>
        </div>

        <div style={{ display: 'flex', gap: 10, flexWrap: 'wrap' }}>
          <button className="btn primary" onClick={create} disabled={!description.trim()}>Adicionar</button>
          <button className="btn" onClick={load}>Recarregar</button>
        </div>
      </div>

      <div className="card-pad">
        <table className="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Descrição</th>
            <th>Finalidade</th>
          </tr>
        </thead>
        <tbody>
          {items.map(c => (
            <tr key={c.id}>
              <td>{c.id}</td>
              <td>{c.description}</td>
              <td>{c.purpose}</td>
            </tr>
          ))}
          {items.length === 0 && (
            <tr><td colSpan={3}>Nenhuma categoria cadastrada.</td></tr>
          )}
        </tbody>
        </table>
      </div>
    </section>
  )
}