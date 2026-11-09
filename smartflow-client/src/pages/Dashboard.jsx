import { useState, useEffect } from "react";
import { Line, Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
import Layout from "../components/Layout";
import { getDashboardStats } from "../api/dashboardApi";

// Enregistrement obligatoire des composants Chart.js qu'on va utiliser
ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, BarElement, Title, Tooltip, Legend);

export default function Dashboard() {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getDashboardStats()
      .then((res) => setStats(res.data))
      .catch((err) => console.error(err))
      .finally(() => setLoading(false));
  }, []);

  if (loading) {
    return (
      <Layout>
        <p className="text-gray-500">Chargement du dashboard...</p>
      </Layout>
    );
  }

  // Transforme les données brutes de l'API au format attendu par Chart.js
  const dataVentesParJour = {
    labels: stats.ventesParJour.map((v) => v.date),
    datasets: [
      {
        label: "Ventes (DH)",
        data: stats.ventesParJour.map((v) => v.montant),
        borderColor: "rgb(37, 99, 235)",
        backgroundColor: "rgba(37, 99, 235, 0.2)",
        tension: 0.3, // arrondit légèrement la courbe
      },
    ],
  };

  const dataTopProduits = {
    labels: stats.topProduits.map((p) => p.produitNom),
    datasets: [
      {
        label: "Quantité vendue",
        data: stats.topProduits.map((p) => p.quantiteVendue),
        backgroundColor: "rgba(34, 197, 94, 0.7)",
      },
    ],
  };

  return (
    <Layout>
      <h1 className="text-2xl font-bold text-gray-800 mb-6">Dashboard</h1>

      {/* Cartes de statistiques principales */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-8">
        <div className="bg-white rounded shadow p-5">
          <p className="text-sm text-gray-500">Chiffre d'affaires total</p>
          <p className="text-2xl font-bold text-blue-600">{stats.chiffreAffairesTotal.toFixed(2)} DH</p>
        </div>
        <div className="bg-white rounded shadow p-5">
          <p className="text-sm text-gray-500">Nombre de ventes</p>
          <p className="text-2xl font-bold text-gray-800">{stats.nombreVentes}</p>
        </div>
        <div className="bg-white rounded shadow p-5">
          <p className="text-sm text-gray-500">Clients</p>
          <p className="text-2xl font-bold text-gray-800">{stats.nombreClients}</p>
        </div>
        <div className="bg-white rounded shadow p-5">
          <p className="text-sm text-gray-500">Produits en stock faible</p>
          <p className={`text-2xl font-bold ${stats.produitsStockFaible > 0 ? "text-red-600" : "text-gray-800"}`}>
            {stats.produitsStockFaible}
          </p>
        </div>
      </div>

      {/* Graphiques */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-white rounded shadow p-5">
          <h2 className="font-semibold text-gray-700 mb-3">Ventes des 7 derniers jours</h2>
          {stats.ventesParJour.length > 0 ? (
            <Line data={dataVentesParJour} />
          ) : (
            <p className="text-gray-400 text-sm">Aucune vente récente à afficher.</p>
          )}
        </div>

        <div className="bg-white rounded shadow p-5">
          <h2 className="font-semibold text-gray-700 mb-3">Top 5 produits vendus</h2>
          {stats.topProduits.length > 0 ? (
            <Bar data={dataTopProduits} />
          ) : (
            <p className="text-gray-400 text-sm">Aucune vente enregistrée.</p>
          )}
        </div>
      </div>
    </Layout>
  );
}