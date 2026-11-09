import { useState, useEffect } from "react";
import Layout from "../components/Layout";
import { getProduits, deleteProduit } from "../api/produitsApi";
import ProduitFormModal from "../components/ProduitFormModal";
import StockMovementModal from "../components/StockMovementModal";

export default function Produits() {
  const [produits, setProduits] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [pageNumber, setPageNumber] = useState(1);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);

  const [modalOuverte, setModalOuverte] = useState(false);
  const [produitAModifier, setProduitAModifier] = useState(null);

  // Gère à la fois "entrée" et "sortie" avec un seul state
  const [mouvementStock, setMouvementStock] = useState(null); // { produit, type } ou null

  useEffect(() => {
    chargerProduits();
  }, [pageNumber, search]);

  const chargerProduits = async () => {
    setLoading(true);
    try {
      const response = await getProduits(pageNumber, 10, search);
      setProduits(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Confirmer la suppression de ce produit ?")) return;
    try {
      await deleteProduit(id);
      chargerProduits();
    } catch (error) {
      alert("Erreur lors de la suppression (droits insuffisants ?).");
    }
  };

  return (
    <Layout>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Produits</h1>
        <button
          onClick={() => { setProduitAModifier(null); setModalOuverte(true); }}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          + Nouveau produit
        </button>
      </div>

      <input
        type="text"
        placeholder="Rechercher un produit..."
        value={search}
        onChange={(e) => { setSearch(e.target.value); setPageNumber(1); }}
        className="w-full border border-gray-300 rounded px-3 py-2 mb-4"
      />

      {loading ? (
        <p className="text-gray-500">Chargement...</p>
      ) : (
        <>
          <table className="w-full bg-white rounded shadow">
            <thead>
              <tr className="border-b text-left text-gray-600 text-sm">
                <th className="p-3">Nom</th>
                <th className="p-3">Référence</th>
                <th className="p-3">Catégorie</th>
                <th className="p-3">Prix vente</th>
                <th className="p-3">Stock</th>
                <th className="p-3">Actions</th>
              </tr>
            </thead>
            <tbody>
              {produits.map((produit) => (
                <tr key={produit.id} className="border-b hover:bg-gray-50">
                  <td className="p-3">{produit.nom}</td>
                  <td className="p-3">{produit.reference}</td>
                  <td className="p-3">{produit.categorieNom}</td>
                  <td className="p-3">{produit.prixVente} DH</td>
                  <td className="p-3">
                    <span className={produit.stockFaible ? "text-red-600 font-semibold" : ""}>
                      {produit.quantiteStock}
                    </span>
                    {produit.stockFaible && (
                      <span className="ml-1 text-xs bg-red-100 text-red-600 px-2 py-0.5 rounded">
                        Stock faible
                      </span>
                    )}
                  </td>
                  <td className="p-3">
                    <div className="flex gap-2 flex-wrap">
                      <button
                        onClick={() => { setProduitAModifier(produit); setModalOuverte(true); }}
                        className="text-blue-600 hover:underline text-sm"
                      >
                        Modifier
                      </button>
                      <button
                        onClick={() => setMouvementStock({ produit, type: "entree" })}
                        className="text-green-600 hover:underline text-sm"
                      >
                        + Stock
                      </button>
                      <button
                        onClick={() => setMouvementStock({ produit, type: "sortie" })}
                        className="text-orange-500 hover:underline text-sm"
                      >
                        - Stock
                      </button>
                      <button
                        onClick={() => handleDelete(produit.id)}
                        className="text-red-500 hover:underline text-sm"
                      >
                        Supprimer
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {produits.length === 0 && (
                <tr><td colSpan="6" className="p-3 text-center text-gray-500">Aucun produit trouvé.</td></tr>
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

      {modalOuverte && (
        <ProduitFormModal
          produitAModifier={produitAModifier}
          onClose={() => setModalOuverte(false)}
          onSuccess={chargerProduits}
        />
      )}

      {mouvementStock && (
        <StockMovementModal
          produit={mouvementStock.produit}
          type={mouvementStock.type}
          onClose={() => setMouvementStock(null)}
          onSuccess={chargerProduits}
        />
      )}
    </Layout>
  );
}