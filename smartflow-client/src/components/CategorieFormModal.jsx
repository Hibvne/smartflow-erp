import { useForm } from "react-hook-form";
import { useEffect } from "react";
import { createCategorie, updateCategorie } from "../api/categoriesApi";

export default function CategorieFormModal({ categorieAModifier, onClose, onSuccess }) {
  const { register, handleSubmit, reset, formState: { errors } } = useForm();

  useEffect(() => {
    if (categorieAModifier) {
      reset(categorieAModifier);
    } else {
      reset({ nom: "", description: "" });
    }
  }, [categorieAModifier]);

  const onSubmit = async (data) => {
    try {
      if (categorieAModifier) {
        await updateCategorie(categorieAModifier.id, data);
      } else {
        await createCategorie(data);
      }
      onSuccess();
      onClose();
    } catch (error) {
      alert("Erreur lors de l'enregistrement.");
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 w-96 shadow-lg">
        <h2 className="text-xl font-bold mb-4">
          {categorieAModifier ? "Modifier la catégorie" : "Nouvelle catégorie"}
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
            <label className="block text-sm font-medium text-gray-700 mb-1">  Préfixe (pour les références produits, ex: VET, FRU) </label>
            <input
             {...register("prefixe", { required: "Le préfixe est requis.", maxLength: 5 })}
             maxLength={5}
             onInput={(e) => (e.target.value = e.target.value.toUpperCase())}
                className="w-full border border-gray-300 rounded px-3 py-2"
                placeholder="VET"
            />
             {errors.prefixe && <p className="text-red-500 text-sm">{errors.prefixe.message}</p>}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">Description</label>
            <textarea
              {...register("description")}
              className="w-full border border-gray-300 rounded px-3 py-2"
              rows="3"
            />
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