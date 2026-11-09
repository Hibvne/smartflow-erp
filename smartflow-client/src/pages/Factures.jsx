import { useState, useEffect } from "react";
import Layout from "../components/Layout";
import { getFactures, enregistrerPaiement } from "../api/facturesApi";

export default function Factures() {
  const [factures, setFactures] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [pageNumber, setPageNumber] = useState(1);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    chargerFactures();
  }, [pageNumber]);

  const chargerFactures = async () => {
    setLoading(true);
    try {
      const response = await getFactures(pageNumber, 10);
      setFactures(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  // Petite fonction utilitaire : associe une couleur au statut, pour un repérage visuel rapide
  const couleurStatut = (statut) => {
    switch (statut) {
      case "Payée": return "bg-green-100 text-green-700";
      case "Annulée": return "bg-red-100 text-red-700";
      default: return "bg-yellow-100 text-yellow-700"; // "En attente"
    }
  };

  const handlePaiement = async (facture) => {
    const modePaiement = window.prompt("Mode de paiement (Espèces / Virement / Carte) :", "Espèces");
    if (!modePaiement) return;

    try {
      await enregistrerPaiement(facture.id, {
        montant: facture.montantTotal,
        modePaiement,
      });
      chargerFactures(); // rafraîchit la liste pour voir le nouveau statut
    } catch (error) {
      alert(error.response?.data || "Erreur lors de l'enregistrement du paiement.");
    }
  };

  return (
    <Layout>
      <h1 className="text-2xl font-bold text-gray-800 mb-6">Factures</h1>

      {loading ? (
        <p className="text-gray-500">Chargement...</p>
      ) : (
        <>
          <table className="w-full bg-white rounded shadow">
            <thead>
              <tr className="border-b text-left text-gray-600 text-sm">
                <th className="p-3">Numéro</th>
                <th className="p-3">Date</th>
                <th className="p-3">Client</th>
                <th className="p-3">Montant</th>
                <th className="p-3">Statut</th>
                <th className="p-3">Actions</th>
              </tr>
            </thead>
            <tbody>
              {factures.map((f) => (
                <tr key={f.id} className="border-b hover:bg-gray-50">
                  <td className="p-3 font-medium">{f.numeroFacture}</td>
                  <td className="p-3">{new Date(f.dateFacture).toLocaleDateString()}</td>
                  <td className="p-3">{f.clientNom}</td>
                  <td className="p-3">{f.montantTotal} DH</td>
                  <td className="p-3">
                    <span className={`text-xs px-2 py-1 rounded ${couleurStatut(f.statut)}`}>
                      {f.statut}
                    </span>
                  </td>
                  <td className="p-3">
                    {f.statut === "En attente" && (
                      <button
                        onClick={() => handlePaiement(f)}
                        className="text-green-600 hover:underline text-sm"
                      >
                        Marquer payée
                      </button>
                    )}
                  </td>
                </tr>
              ))}
              {factures.length === 0 && (
                <tr><td colSpan="6" className="p-3 text-center text-gray-500">Aucune facture.</td></tr>
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