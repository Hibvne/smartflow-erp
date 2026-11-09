import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

// Ce composant "enveloppe" une page : si pas connecté -> redirige vers /login
// sinon -> affiche la page demandée normalement
export default function ProtectedRoute({ children }) {
  const { user, loading } = useAuth();

  if (loading) {
    // Le temps que AuthContext vérifie le token au chargement (voir useEffect vu avant)
    return <div className="min-h-screen flex items-center justify-center">Chargement...</div>;
  }

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  return children;
}