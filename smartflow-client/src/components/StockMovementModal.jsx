import { useForm } from "react-hook-form";
import { entreeStock, sortieStock } from "../api/produitsApi";

// type = "entree" ou "sortie"
export default function StockMovementModal({ produit, type, onClose, onSuccess }) {
  const { register, handleSubmit, formState: { errors } } = useForm();

  const onSubmit = async (data) => {
    const payload = { quantite: parseInt(data.quantite), motif: data.motif };

    try {
      if (type === "entree") {
        await entreeStock(produit.id, payload);
      } else {
        await sortieStock(produit.id, payload);
      }
      onSuccess();
      onClose();
    } catch (error) {
      // Le backend renvoie un message clair (ex: "Stock insuffisant") -> on l'affiche tel quel
      alert(error.response?.data || "Erreur lors de l'opération.");
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 w-80 shadow-lg">
        <h2 className="text-lg font-bold mb-1">
          {type === "entree" ? "Entrée de stock" : "Sortie de stock"}
        </h2>
        <p className="text-sm text-gray-500 mb-4">{produit.nom} (stock actuel : {produit.quantiteStock})</p>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="mb-3">
            <label className="block text-sm font-medium text-gray-700 mb-1">Quantité</label>
            <input
              type="number"
              {...register("quantite", { required: true, min: 1 })}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>
          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">Motif</label>
            <input
              {...register("motif", { required: "Le motif est requis." })}
              placeholder={type === "entree" ? "Ex: Réapprovisionnement" : "Ex: Casse, Inventaire"}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
            {errors.motif && <p className="text-red-500 text-sm">{errors.motif.message}</p>}
          </div>

          <div className="flex justify-end gap-2">
            <button type="button" onClick={onClose} className="px-4 py-2 rounded border border-gray-300 hover:bg-gray-50">
              Annuler
            </button>
            <button
              type="submit"
              className={`px-4 py-2 rounded text-white ${type === "entree" ? "bg-green-600 hover:bg-green-700" : "bg-orange-500 hover:bg-orange-600"}`}
            >
              Confirmer
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}