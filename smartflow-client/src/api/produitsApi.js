import axiosClient from "./axiosClient";

export const getProduits = (pageNumber = 1, pageSize = 10, search = "") => {
  return axiosClient.get("/Produits", {
    params: { pageNumber, pageSize, search },
  });
};

export const getProduitById = (id) => {
  return axiosClient.get(`/Produits/${id}`);
};

export const createProduit = (data) => {
  return axiosClient.post("/Produits", data);
};

export const updateProduit = (id, data) => {
  return axiosClient.put(`/Produits/${id}`, data);
};

export const deleteProduit = (id) => {
  return axiosClient.delete(`/Produits/${id}`);
};

// Endpoints spécifiques à la gestion de stock
export const entreeStock = (id, data) => {
  return axiosClient.post(`/Produits/${id}/entree-stock`, data);
};

export const sortieStock = (id, data) => {
  return axiosClient.post(`/Produits/${id}/sortie-stock`, data);
};