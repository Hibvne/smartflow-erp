import { Link, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Navbar() {
  const { user, logout } = useAuth();
  const location = useLocation(); // permet de savoir sur quelle page on est actuellement

  // Liste des liens de navigation, centralisée ici pour être facile à modifier plus tard
  const links = [
    { path: "/dashboard", label: "Dashboard" },
    { path: "/clients", label: "Clients" },
    { path: "/produits", label: "Produits" },
    { path: "/ventes", label: "Ventes" },
    { path: "/factures", label: "Factures" },
    { path: "/categories", label: "Catégories" },
    
  ];

  return (
    <nav className="bg-white shadow-md px-6 py-4 flex justify-between items-center">
      <h1 className="text-xl font-bold text-blue-600">SmartFlow ERP</h1>

      <div className="flex gap-6">
        {links.map((link) => (
          <Link
            key={link.path}
            to={link.path}
            // Style conditionnel : le lien de la page actuelle est mis en évidence
            className={`font-medium ${
              location.pathname === link.path
                ? "text-blue-600 border-b-2 border-blue-600"
                : "text-gray-600 hover:text-blue-600"
            }`}
          >
            {link.label}
          </Link>
        ))}
      </div>

      <div className="flex items-center gap-4">
        <span className="text-sm text-gray-600">
          {user?.email} <span className="text-gray-400">({user?.role})</span>
        </span>
        <button
          onClick={logout}
          className="bg-red-500 text-white px-3 py-1.5 rounded text-sm hover:bg-red-600"
        >
          Déconnexion
        </button>
      </div>
    </nav>
  );
}