import { useForm } from "react-hook-form";
import { useEffect } from "react";
import { createClient, updateClient } from "../api/clientsApi";

// clientAModifier = null si on crée un nouveau client, ou l'objet client si on modifie
export default function ClientFormModal({ clientAModifier, onClose, onSuccess }) {
  const { register, handleSubmit, reset, formState: { errors } } = useForm();

  // Si on ouvre la modale en mode "modification", on pré-remplit le formulaire
  useEffect(() => {
    if (clientAModifier) {
      reset(clientAModifier); // remplit tous les champs d'un coup avec les valeurs existantes
    } else {
      reset({ nom: "", email: "", telephone: "", adresse: "" });
    }
  }, [clientAModifier]);

  const onSubmit = async (data) => {
    try {
      if (clientAModifier) {
        await updateClient(clientAModifier.id, data);
      } else {
        await createClient(data);
      }
      onSuccess(); // recharge la liste dans la page parente
      onClose();   // ferme la modale
    } catch (error) {
      alert("Erreur lors de l'enregistrement.");
    }
  };

  return (
    // Fond semi-transparent qui couvre toute la page, derrière la modale
    <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 w-96 shadow-lg">
        <h2 className="text-xl font-bold mb-4">
          {clientAModifier ? "Modifier le client" : "Nouveau client"}
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

          <div className="mb-3">
            <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
            <input
              {...register("email", { required: "L'email est requis." })}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
            {errors.email && <p className="text-red-500 text-sm">{errors.email.message}</p>}
          </div>

          <div className="mb-3">
            <label className="block text-sm font-medium text-gray-700 mb-1">Téléphone</label>
            <input
              {...register("telephone")}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">Adresse</label>
            <input
              {...register("adresse")}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          <div className="flex justify-end gap-2">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 rounded border border-gray-300 hover:bg-gray-50"
            >
              Annuler
            </button>
            <button
              type="submit"
              className="px-4 py-2 rounded bg-blue-600 text-white hover:bg-blue-700"
            >
              Enregistrer
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}