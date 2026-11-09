import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import ProtectedRoute from "./routes/ProtectedRoute";
import Login from "./pages/Login";
import Clients from "./pages/Clients";
import Dashboard from "./pages/Dashboard";
import Produits from "./pages/Produits";
import Categories from "./pages/Categories";
import Ventes from "./pages/Ventes";
import NouvelleVente from "./pages/NouvelleVente";
import Factures from "./pages/Factures";

function App() {
  return (
    // AuthProvider enveloppe TOUTE l'app -> le Context est accessible partout
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />

          <Route
            path="/dashboard"
            element={
              <ProtectedRoute>
                <Dashboard />
              </ProtectedRoute>
            }
          />

          <Route
            path="/clients"
            element={
             <ProtectedRoute> 
               <Clients />
             </ProtectedRoute>
            }
          />

          <Route
            path="/produits"
            element={
             <ProtectedRoute>
               <Produits />
             </ProtectedRoute>
            }
          />

          <Route
            path="/categories"
            element={
             <ProtectedRoute>
               <Categories />
             </ProtectedRoute>
           }
         />

         <Route path="/ventes" element={<ProtectedRoute><Ventes /></ProtectedRoute>} />
         <Route path="/ventes/nouvelle" element={<ProtectedRoute><NouvelleVente /></ProtectedRoute>} />

         <Route path="/factures" element={<ProtectedRoute><Factures /></ProtectedRoute>} />




          {/* Redirige la racine "/" vers /dashboard (qui redirigera lui-même vers /login si pas connecté) */}
          <Route path="/" element={<Navigate to="/dashboard" replace />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;