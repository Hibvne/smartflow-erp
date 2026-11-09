import axios from "axios";

// URL de base de ton API .NET (vérifie que le port correspond à celui affiché par Visual Studio au démarrage)
const axiosClient = axios.create({
  baseURL: "https://localhost:7298/api",
});

// Intercepteur : s'exécute AVANT chaque requête envoyée
// Il ajoute automatiquement le token JWT dans le header, sans qu'on ait à y penser à chaque appel
axiosClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default axiosClient;  