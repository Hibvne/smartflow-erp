import { useState, useEffect } from "react";
import Layout from "../components/Layout";
import { getCategories, deleteCategorie } from "../api/categoriesApi";
import CategorieFormModal from "../components/CategorieFormModal";

export default function Categories() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalOuverte, setModalOuverte] = useState(false);
  const [categorieAModifier, setCategorieAModifier] = useState(null);

  useEffect(() => {
    chargerCategories();
  }, []);

  const chargerCategories = async () => {
    setLoading(true);
    try {
      const response = await getCategories();
      setCategories(response.data);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Confirmer la suppression de cette catégorie ?")) return;
    try {
      await deleteCategorie(id);
      chargerCategories();
    } catch (error) {
      alert("Erreur lors de la suppression (droits insuffisants, ou catégorie utilisée par des produits).");
    }
  };

  return (
    <Layout>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Catégories</h1>
        <button
          onClick={() => { setCategorieAModifier(null); setModalOuverte(true); }}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          + Nouvelle catégorie
        </button>
      </div>

      {loading ? (
        <p className="text-gray-500">Chargement...</p>
      ) : (
        <table className="w-full bg-white rounded shadow">
          <thead>
            <tr className="border-b text-left text-gray-600 text-sm">
              <th className="p-3">Nom</th>
              <th className="p-3">Description</th>
              <th className="p-3">Actions</th>
            </tr>
          </thead>
          <tbody>
            {categories.map((cat) => (
              <tr key={cat.id} className="border-b hover:bg-gray-50">
                <td className="p-3">{cat.nom}</td>
                <td className="p-3 text-gray-500">{cat.description}</td>
                <td className="p-3 flex gap-2">
                  <button
                    onClick={() => { setCategorieAModifier(cat); setModalOuverte(true); }}
                    className="text-blue-600 hover:underline text-sm"
                  >
                    Modifier
                  </button>
                  <button
                    onClick={() => handleDelete(cat.id)}
                    className="text-red-500 hover:underline text-sm"
                  >
                    Supprimer
                  </button>
                </td>
              </tr>
            ))}
            {categories.length === 0 && (
              <tr><td colSpan="3" className="p-3 text-center text-gray-500">Aucune catégorie.</td></tr>
            )}
          </tbody>
        </table>
      )}

      {modalOuverte && (
        <CategorieFormModal
          categorieAModifier={categorieAModifier}
          onClose={() => setModalOuverte(false)}
          onSuccess={chargerCategories}
        />
      )}
    </Layout>
  );
}