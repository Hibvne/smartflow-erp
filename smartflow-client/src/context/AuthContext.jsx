import { createContext, useContext, useState, useEffect } from "react";
import axiosClient from "../api/axiosClient";

// La "boîte globale" elle-même, vide au départ
const AuthContext = createContext();

// Composant qui va "envelopper" toute l'app pour rendre le contexte disponible partout
export function AuthProvider({ children }) {
  const [user, setUser] = useState(null); // { email, role } ou null si pas connecté
  const [loading, setLoading] = useState(true); // true tant qu'on vérifie si un token existe déjà

  // S'exécute une seule fois, au tout premier chargement de l'app
  useEffect(() => {
    const token = localStorage.getItem("token");

    if (token) {
      // Un token existe déjà (ex: after F5) -> on redemande au backend qui est connecté
      axiosClient
        .get("/Auth/me")
        .then((response) => setUser(response.data))
        .catch(() => {
          // Token invalide/expiré -> on nettoie
          localStorage.removeItem("token");
          setUser(null);
        })
        .finally(() => setLoading(false));
    } else {
      setLoading(false);
    }
  }, []);

  // Appelée depuis la page Login après un login réussi
  const login = (token, userData) => {
    localStorage.setItem("token", token);
    setUser(userData);
  };

  const logout = () => {
    localStorage.removeItem("token");
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

// Petit raccourci pratique pour utiliser le contexte dans n'importe quel composant
export function useAuth() {
  return useContext(AuthContext);
}