import { useState } from "react";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import axiosClient from "../api/axiosClient";
import { useAuth } from "../context/AuthContext";

export default function Login() {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [erreurServeur, setErreurServeur] = useState("");
  const { login } = useAuth();
  const navigate = useNavigate();

  // Appelée uniquement si la validation du formulaire (côté client) réussit
  const onSubmit = async (data) => {
    setErreurServeur("");
    try {
      const response = await axiosClient.post("/Auth/login", {
        email: data.email,
        motDePasse: data.motDePasse,
      });

      const token = response.data.token;

      // On redemande /me juste après pour récupérer email + rôle proprement
      localStorage.setItem("token", token); // temporairement, pour que /me puisse l'utiliser
      const meResponse = await axiosClient.get("/Auth/me");

      login(token, meResponse.data); // met à jour le Context global
      navigate("/dashboard"); // redirige après connexion réussie
    } catch (error) {
      setErreurServeur("Email ou mot de passe incorrect.");
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center">
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="bg-white p-8 rounded-lg shadow-md w-96"
      >
        <h1 className="text-2xl font-bold text-blue-600 mb-6 text-center">
          SmartFlow ERP
        </h1>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Email
          </label>
          <input
            type="email"
            {...register("email", { required: "L'email est requis." })}
            className="w-full border border-gray-300 rounded px-3 py-2"
          />
          {errors.email && (
            <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>
          )}
        </div>

        <div className="mb-4">
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Mot de passe
          </label>
          <input
            type="password"
            {...register("motDePasse", { required: "Le mot de passe est requis." })}
            className="w-full border border-gray-300 rounded px-3 py-2"
          />
          {errors.motDePasse && (
            <p className="text-red-500 text-sm mt-1">{errors.motDePasse.message}</p>
          )}
        </div>

        {erreurServeur && (
          <p className="text-red-500 text-sm mb-4 text-center">{erreurServeur}</p>
        )}

        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition"
        >
          Se connecter
        </button>
      </form>
    </div>
  );
}