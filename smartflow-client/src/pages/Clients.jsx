import { useState, useEffect } from "react";
import Layout from "../components/Layout";
import { getClients, deleteClient } from "../api/clientsApi";
import ClientFormModal from "../components/ClientFormModal";

export default function Clients() {
  const [clients, setClients] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [pageNumber, setPageNumber] = useState(1);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const [modalOuverte, setModalOuverte] = useState(false);
  const [clientAModifier, setClientAModifier] = useState(null);

  // Se déclenche à chaque fois que pageNumber ou search change
  useEffect(() => {
    chargerClients();
  }, [pageNumber, search]);

  const chargerClients = async () => {
    setLoading(true);
    try {
      const response = await getClients(pageNumber, 10, search);
      setClients(response.data.items);
      setTotalPages(response.data.totalPages);
    } catch (error) {
      console.error("Erreur lors du chargement des clients :", error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Confirmer la suppression de ce client ?")) return;

    try {
      await deleteClient(id);
      chargerClients(); // recharge la liste après suppression
    } catch (error) {
      alert("Erreur lors de la suppression (droits insuffisants ?).");
    }
  };

  return (
    <Layout>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold text-gray-800">Clients</h1>

        <button // ouvre la modale en mode création
           onClick={() => {
           setClientAModifier(null); // null = mode création
           setModalOuverte(true);
            }}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
           + Nouveau client
        </button>

      </div>

      <input
        type="text"
        placeholder="Rechercher un client..."
        value={search}
        onChange={(e) => {
          setSearch(e.target.value);
          setPageNumber(1); // on revient à la page 1 à chaque nouvelle recherche
        }}
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
                <th className="p-3">Email</th>
                <th className="p-3">Téléphone</th>
                <th className="p-3">Actions</th>
              </tr>
            </thead>
            <tbody>
              {clients.map((client) => (
                <tr key={client.id} className="border-b hover:bg-gray-50">
                  <td className="p-3">{client.nom}</td>
                  <td className="p-3">{client.email}</td>
                  <td className="p-3">{client.telephone}</td>
                  <td className="p-3 flex gap-2">
                    <button
                      onClick={() => {
                      setClientAModifier(client); // on passe le client existant = mode modification
                      setModalOuverte(true);
                      }}
                      className="text-blue-600 hover:underline text-sm"
                     >
                        Modifier
                    </button>
                    <button
                      onClick={() => handleDelete(client.id)}
                      className="text-red-500 hover:underline text-sm"
                    >
                      Supprimer
                    </button>
                  </td>
                </tr>
              ))}
              {clients.length === 0 && (
                <tr>
                  <td colSpan="4" className="p-3 text-center text-gray-500">
                    Aucun client trouvé.
                  </td>
                </tr>
              )}
            </tbody>
          </table>

          {/* Pagination */}
          <div className="flex justify-center gap-2 mt-4">
            <button
              disabled={pageNumber === 1}
              onClick={() => setPageNumber((p) => p - 1)}
              className="px-3 py-1 border rounded disabled:opacity-40"
            >
              Précédent
            </button>
            <span className="px-3 py-1">
              Page {pageNumber} / {totalPages}
            </span>
            <button
              disabled={pageNumber === totalPages}
              onClick={() => setPageNumber((p) => p + 1)}
              className="px-3 py-1 border rounded disabled:opacity-40"
            >
              Suivant
            </button>
          </div>
        </>
      )}

      {modalOuverte && (
       <ClientFormModal
         clientAModifier={clientAModifier}
         onClose={() => setModalOuverte(false)}
         onSuccess={chargerClients}
        />
        )}


    </Layout>
  );
}