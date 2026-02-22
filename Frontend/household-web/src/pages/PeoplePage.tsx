import { useEffect, useState } from 'react'
import { api } from '../api/endpoints'
import type { PersonListDto } from '../types/person'

export default function PeoplePage() {
  const [items, setItems] = useState<PersonListDto[]>([])
  const [name, setName] = useState('')
  const [age, setAge] = useState<number>(0)
  const [error, setError] = useState<string | null>(null)

  async function load() {
    setError(null)
    try {
      setItems(await api.people.list())
    } catch (e: any) {
      setError(e.message)
    }
  }

  async function create() {
    setError(null)
    try {
      await api.people.create({ name, age })
      setName('')
      setAge(0)
      await load()
    } catch (e: any) {
      setError(e.message)
    }
  }

  async function remove(id: number) {
    setError(null)
    try {
      await api.people.remove(id)
      await load()
    } catch (e: any) {
      setError(e.message)
    }
  }

  useEffect(() => { load() }, [])

  return (
    <section className="page">
      <h2>Pessoas</h2>

      {error && <div className="alert">{error}</div>}

      <div className="toolbar">
        <div style={{ display: 'flex', gap: 10, flexWrap: 'wrap', alignItems: 'center', width: '100%' }}>
          <input className="input" placeholder="Nome" value={name} onChange={e => setName(e.target.value)} />
          <input
            className="input"
            placeholder="Idade"
            type="number"
            value={age}
            onChange={e => setAge(Number(e.target.value))}
            style={{ width: 140 }}
          />
        </div>

        <div style={{ display: 'flex', gap: 10, flexWrap: 'wrap' }}>
          <button className="btn primary" onClick={create} disabled={!name.trim()}>Adicionar</button>
          <button className="btn" onClick={load}>Recarregar</button>
        </div>
      </div>

      <div className="card-pad">
        <table className="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nome</th>
            <th>Idade</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {items.map(p => (
            <tr key={p.id}>
              <td>{p.id}</td>
              <td>{p.name}</td>
              <td>{p.age}</td>
              <td>
                <button className="btn danger" onClick={() => remove(p.id)}>Excluir</button>
              </td>
            </tr>
          ))}
          {items.length === 0 && (
            <tr><td colSpan={4}>Nenhuma pessoa cadastrada.</td></tr>
          )}
        </tbody>
        </table>

        <div style={{ marginTop: 10, color: 'var(--muted)', fontSize: 12 }}>
          Ao excluir uma pessoa, suas transações são removidas em cascata no banco.
        </div>
      </div>
    </section>
  )
}