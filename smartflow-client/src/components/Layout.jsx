import Navbar from "./Navbar";

// {children} = le contenu de la page actuelle, injecté à l'intérieur du layout
export default function Layout({ children }) {
  return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <main className="p-8">{children}</main>
    </div>
  );
}