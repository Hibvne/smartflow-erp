import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Layout from "../components/Layout";
import { getVentes } from "../api/ventesApi";

export default function Ventes() {
  const navigate = useNavigate();
  const [ventes, setVentes] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [pageNumber, setPageNumber] = useState(1);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    chargerVentes();
  }, [pageNumber]);

  const chargerVentes = async () => {
    setLoading(true);
    try {
      const response = await getVentes(pageNumber, 10);
      setVentes(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Layout>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Ventes</h1>
        <button
          onClick={() => navigate("/ventes/nouvelle")}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          + Nouvelle vente
        </button>
      </div>

      {loading ? (
        <p className="text-gray-500">Chargement...</p>
      ) : (
        <>
          <table className="w-full bg-white rounded shadow">
            <thead>
              <tr className="border-b text-left text-gray-600 text-sm">
                <th className="p-3">Date</th>
                <th className="p-3">Client</th>
                <th className="p-3">Vendeur</th>
                <th className="p-3">Montant</th>
                <th className="p-3">Facture</th>
              </tr>
            </thead>
            <tbody>
              {ventes.map((vente) => (
                <tr key={vente.id} className="border-b hover:bg-gray-50">
                  <td className="p-3">{new Date(vente.dateVente).toLocaleDateString()}</td>
                  <td className="p-3">{vente.clientNom}</td>
                  <td className="p-3">{vente.vendeurNom}</td>
                  <td className="p-3 font-medium">{vente.montantTotal} DH</td>
                  <td className="p-3">{vente.numeroFacture}</td>
                </tr>
              ))}
              {ventes.length === 0 && (
                <tr><td colSpan="5" className="p-3 text-center text-gray-500">Aucune vente.</td></tr>
              )}
            </tbody>
          </table>

          <div className="flex justify-center gap-2 mt-4">
            <button disabled={pageNumber === 1} onClick={() => setPageNumber((p) => p - 1)} className="px-3 py-1 border rounded disabled:opacity-40">
              Précédent
            </button>
            <span className="px-3 py-1">Page {pageNumber} / {totalPages}</span>
            <button disabled={pageNumber === totalPages} onClick={() => setPageNumber((p) => p + 1)} className="px-3 py-1 border rounded disabled:opacity-40">
              Suivant
            </button>
          </div>
        </>
      )}
    </Layout>
  );
}