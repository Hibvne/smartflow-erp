import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Layout from "../components/Layout";
import { getClients } from "../api/clientsApi";
import { getProduits } from "../api/produitsApi";
import { createVente } from "../api/ventesApi";

export default function NouvelleVente() {
  const navigate = useNavigate();

  const [clients, setClients] = useState([]);
  const [produits, setProduits] = useState([]);
  const [clientId, setClientId] = useState("");
  const [panier, setPanier] = useState([]); // [{ produitId, produitNom, quantite, prixUnitaire }]
  const [produitSelectionne, setProduitSelectionne] = useState("");
  const [quantiteSaisie, setQuantiteSaisie] = useState(1);
  const [erreur, setErreur] = useState("");

  // Charge les listes nécessaires (clients + produits) une seule fois au chargement
  useEffect(() => {
    getClients(1, 1000).then((res) => setClients(res.data.items)); // pageSize élevé pour tout récupérer
    getProduits(1, 1000).then((res) => setProduits(res.data.items));
  }, []);

  const ajouterAuPanier = () => {
    setErreur("");
    if (!produitSelectionne) {
      setErreur("Choisis un produit.");
      return;
    }

    const produit = produits.find((p) => p.id === parseInt(produitSelectionne));

    // Vérification côté frontend : ne pas dépasser le stock disponible
    if (quantiteSaisie > produit.quantiteStock) {
      setErreur(`Stock insuffisant pour "${produit.nom}" (disponible : ${produit.quantiteStock}).`);
      return;
    }

    // Si le produit est déjà dans le panier, on augmente juste la quantité au lieu de dupliquer la ligne
    const ligneExistante = panier.find((l) => l.produitId === produit.id);
    if (ligneExistante) {
      setPanier(panier.map((l) =>
        l.produitId === produit.id
          ? { ...l, quantite: l.quantite + parseInt(quantiteSaisie) }
          : l
      ));
    } else {
      setPanier([...panier, {
        produitId: produit.id,
        produitNom: produit.nom,
        quantite: parseInt(quantiteSaisie),
        prixUnitaire: produit.prixVente,
      }]);
    }

    setProduitSelectionne("");
    setQuantiteSaisie(1);
  };

  const retirerDuPanier = (produitId) => {
    setPanier(panier.filter((l) => l.produitId !== produitId));
  };

  // Recalculé automatiquement à chaque changement du panier (pas besoin de useEffect pour ça)
  const total = panier.reduce((somme, ligne) => somme + ligne.quantite * ligne.prixUnitaire, 0);

  const validerVente = async () => {
    setErreur("");
    if (!clientId) {
      setErreur("Choisis un client.");
      return;
    }
    if (panier.length === 0) {
      setErreur("Le panier est vide.");
      return;
    }

    try {
      const payload = {
        clientId: parseInt(clientId),
        lignes: panier.map((l) => ({ produitId: l.produitId, quantite: l.quantite })),
      };
      await createVente(payload);
      navigate("/ventes"); // retour à la liste après succès
    } catch (error) {
      // Le backend renvoie un message clair en cas de stock insuffisant détecté côté serveur
      setErreur(error.response?.data || "Erreur lors de la création de la vente.");
    }
  };

  return (
    <Layout>
      <h1 className="text-2xl font-bold text-gray-800 mb-6">Nouvelle vente</h1>

      <div className="bg-white rounded shadow p-6 mb-6">
        <label className="block text-sm font-medium text-gray-700 mb-1">Client</label>
        <select
          value={clientId}
          onChange={(e) => setClientId(e.target.value)}
          className="w-full border border-gray-300 rounded px-3 py-2"
        >
          <option value="">-- Choisir un client --</option>
          {clients.map((c) => (
            <option key={c.id} value={c.id}>{c.nom}</option>
          ))}
        </select>
      </div>

      <div className="bg-white rounded shadow p-6 mb-6">
        <h2 className="font-semibold text-gray-700 mb-3">Ajouter un produit</h2>
        <div className="flex gap-3 items-end">
          <div className="flex-1">
            <label className="block text-sm font-medium text-gray-700 mb-1">Produit</label>
            <select
              value={produitSelectionne}
              onChange={(e) => setProduitSelectionne(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2"
            >
              <option value="">-- Choisir --</option>
              {produits.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.nom} ({p.reference}) - Stock: {p.quantiteStock} - {p.prixVente} DH
                </option>
              ))}
            </select>
          </div>
          <div className="w-24">
            <label className="block text-sm font-medium text-gray-700 mb-1">Qté</label>
            <input
              type="number"
              min="1"
              value={quantiteSaisie}
              onChange={(e) => setQuantiteSaisie(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>
          <button
            onClick={ajouterAuPanier}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            Ajouter
          </button>
        </div>
      </div>

      <div className="bg-white rounded shadow p-6 mb-6">
        <h2 className="font-semibold text-gray-700 mb-3">Panier</h2>
        {panier.length === 0 ? (
          <p className="text-gray-500 text-sm">Aucun produit ajouté.</p>
        ) : (
          <table className="w-full">
            <thead>
              <tr className="border-b text-left text-gray-600 text-sm">
                <th className="p-2">Produit</th>
                <th className="p-2">Quantité</th>
                <th className="p-2">Prix unitaire</th>
                <th className="p-2">Sous-total</th>
                <th className="p-2"></th>
              </tr>
            </thead>
            <tbody>
              {panier.map((ligne) => (
                <tr key={ligne.produitId} className="border-b">
                  <td className="p-2">{ligne.produitNom}</td>
                  <td className="p-2">{ligne.quantite}</td>
                  <td className="p-2">{ligne.prixUnitaire} DH</td>
                  <td className="p-2 font-medium">{(ligne.quantite * ligne.prixUnitaire).toFixed(2)} DH</td>
                  <td className="p-2">
                    <button
                      onClick={() => retirerDuPanier(ligne.produitId)}
                      className="text-red-500 hover:underline text-sm"
                    >
                      Retirer
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}

        <div className="text-right mt-4 text-lg font-bold text-gray-800">
          Total : {total.toFixed(2)} DH
        </div>
      </div>

      {erreur && <p className="text-red-500 mb-4">{erreur}</p>}

      <div className="flex justify-end gap-3">
        <button
          onClick={() => navigate("/ventes")}
          className="px-4 py-2 rounded border border-gray-300 hover:bg-gray-50"
        >
          Annuler
        </button>
        <button
          onClick={validerVente}
          className="px-6 py-2 rounded bg-green-600 text-white hover:bg-green-700"
        >
          Valider la vente
        </button>
      </div>
    </Layout>
  );
}