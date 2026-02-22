import { useEffect, useState } from 'react'
import { api } from '../api/endpoints'
import type { TotalsByPersonReportDto } from '../types/report'

function money(n: number) {
  return n.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

export default function ReportByPersonPage() {
  const [data, setData] = useState<TotalsByPersonReportDto | null>(null)
  const [error, setError] = useState<string | null>(null)

  async function load() {
    setError(null)
    try {
      setData(await api.reports.totalsByPerson())
    } catch (e: any) {
      setError(e.message)
    }
  }

  useEffect(() => { load() }, [])

  return (
    <section className="page">
      <h2>Relatório - Totais por Pessoa</h2>

      {error && <div className="alert">{error}</div>}

      <div className="toolbar">
        <div style={{ color: 'var(--muted)', fontSize: 12 }}>
          Saldo = receitas - despesas.
        </div>
        <button className="btn" onClick={load}>Recarregar</button>
      </div>

      {!data && !error && <div className="card-pad" style={{ color: 'var(--muted)' }}>Carregando...</div>}

      {data && (
        <div className="card-pad">
          <table className="table">
            <thead>
              <tr>
                <th>Pessoa</th>
                <th>Idade</th>
                <th className="right">Total Receitas</th>
                <th className="right">Total Despesas</th>
                <th className="right">Saldo</th>
              </tr>
            </thead>
            <tbody>
              {data.itens.map(i => (
                <tr key={i.personId}>
                  <td>{i.name}</td>
                  <td>{i.age}</td>
                  <td className="right">{money(i.totalReceitas)}</td>
                  <td className="right">{money(i.totalDespesas)}</td>
                  <td className="right">{money(i.saldo)}</td>
                </tr>
              ))}
              {data.itens.length === 0 && (
                <tr><td colSpan={5}>Nenhuma pessoa cadastrada.</td></tr>
              )}
            </tbody>
            <tfoot>
              <tr>
                <td colSpan={2}><b>Total Geral</b></td>
                <td className="right"><b>{money(data.totalGeral.totalReceitas)}</b></td>
                <td className="right"><b>{money(data.totalGeral.totalDespesas)}</b></td>
                <td className="right"><b>{money(data.totalGeral.saldo)}</b></td>
              </tr>
            </tfoot>
          </table>
          <div style={{ marginTop: 10, color: 'var(--muted)', fontSize: 12 }}>
            Pessoas sem transações aparecem com zero.
          </div>
        </div>
      )}
    </section>
  )
}