import { NavLink, Route, Routes } from 'react-router-dom'
import PeoplePage from './pages/PeoplePage'
import CategoriesPage from './pages/CategoriesPage'
import TransactionsPage from './pages/TransactionsPage'
import ReportByPersonPage from './pages/ReportByPersonPage'

export default function App() {
  return (
    <div className="container">
      <header className="card header">
        <div className="brand" aria-label="Controle de gastos">
          <span className="dot" aria-hidden="true" />
          <div>
            <h1>Controle de Gastos</h1>
            <div className="sub">Pessoas, categorias, transações e totais</div>
          </div>
        </div>
      </header>

      <div className="shell" style={{ marginTop: 16 }}>
        <aside className="card">
          <nav className="nav" aria-label="Navegação">
            <NavLink to="/pessoas">👤 Pessoas</NavLink>
            <NavLink to="/categorias">🏷️ Categorias</NavLink>
            <NavLink to="/transacoes">💸 Transações</NavLink>
            <NavLink to="/relatorios/pessoas">📊 Relatório</NavLink>
          </nav>
        </aside>

        <main className="card">
          <Routes>
            <Route path="/pessoas" element={<PeoplePage />} />
            <Route path="/categorias" element={<CategoriesPage />} />
            <Route path="/transacoes" element={<TransactionsPage />} />
            <Route path="/relatorios/pessoas" element={<ReportByPersonPage />} />
            <Route path="*" element={<PeoplePage />} />
          </Routes>
        </main>
      </div>
    </div>
  )
}