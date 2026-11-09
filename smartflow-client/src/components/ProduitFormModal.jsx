import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { createProduit, updateProduit } from "../api/produitsApi";
import { getCategories } from "../api/categoriesApi";

export default function ProduitFormModal({ produitAModifier, onClose, onSuccess }) {
  const { register, handleSubmit, reset, formState: { errors } } = useForm();
  const [categories, setCategories] = useState([]);

  // Charge la liste des catégories une seule fois, au montage de la modale
  useEffect(() => {
    getCategories().then((res) => setCategories(res.data));
  }, []);

  useEffect(() => {
    if (produitAModifier) {
      reset(produitAModifier);
    } else {
      reset({
        nom: "", prixAchat: 0, prixVente: 0,
        quantiteStock: 0, seuilAlerte: 5, categorieId: "",
      });
    }
  }, [produitAModifier]);

  const onSubmit = async (data) => {
    // Les valeurs des inputs number/select arrivent en texte -> on les convertit explicitement
    const payload = {
      ...data,
      prixAchat: parseFloat(data.prixAchat),
      prixVente: parseFloat(data.prixVente),
      quantiteStock: parseInt(data.quantiteStock),
      seuilAlerte: parseInt(data.seuilAlerte),
      categorieId: parseInt(data.categorieId),
    };

    try {
      if (produitAModifier) {
        await updateProduit(produitAModifier.id, payload);
      } else {
        await createProduit(payload);
      }
      onSuccess();
      onClose();
    } catch (error) {
      alert("Erreur lors de l'enregistrement.");
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 w-[28rem] shadow-lg max-h-[90vh] overflow-y-auto">
        <h2 className="text-xl font-bold mb-4">
          {produitAModifier ? "Modifier le produit" : "Nouveau produit"}
        </h2>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="mb-3">
            <label className="block text-sm font-medium text-gray-700 mb-1">Nom</label>
            <input
              {...register("nom", { required: "Le nom est requis." })}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
            {errors.nom && <p className="text-red-500 text-sm">{errors.nom.message}</p>}
          </div>


          <div className="grid grid-cols-2 gap-3 mb-3">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Prix d'achat</label>
              <input
                type="number" step="0.01"
                {...register("prixAchat", { required: true, min: 0 })}
                className="w-full border border-gray-300 rounded px-3 py-2"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Prix de vente</label>
              <input
                type="number" step="0.01"
                {...register("prixVente", { required: true, min: 0 })}
                className="w-full border border-gray-300 rounded px-3 py-2"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3 mb-3">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Stock initial</label>
              <input
                type="number"
                {...register("quantiteStock", { required: true, min: 0 })}
                // Désactivé en modification : le stock se gère via les routes entrée/sortie, pas ici
                disabled={!!produitAModifier}
                className="w-full border border-gray-300 rounded px-3 py-2 disabled:bg-gray-100"
              />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Seuil d'alerte</label>
              <input
                type="number"
                {...register("seuilAlerte", { required: true, min: 0 })}
                className="w-full border border-gray-300 rounded px-3 py-2"
              />
            </div>
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">Catégorie</label>
            <select
              {...register("categorieId", { required: "La catégorie est requise." })}
              className="w-full border border-gray-300 rounded px-3 py-2"
            >
              <option value="">-- Choisir --</option>
              {categories.map((cat) => (
                <option key={cat.id} value={cat.id}>{cat.nom}</option>
              ))}
            </select>
            {errors.categorieId && <p className="text-red-500 text-sm">{errors.categorieId.message}</p>}
          </div>

          <div className="flex justify-end gap-2">
            <button type="button" onClick={onClose} className="px-4 py-2 rounded border border-gray-300 hover:bg-gray-50">
              Annuler
            </button>
            <button type="submit" className="px-4 py-2 rounded bg-blue-600 text-white hover:bg-blue-700">
              Enregistrer
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}