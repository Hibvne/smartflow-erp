import axiosClient from "./axiosClient";

// Centralise tous les appels réseau liés aux Clients
// Les pages n'appellent jamais axiosClient directement, toujours via ce fichier
export const getClients = (pageNumber = 1, pageSize = 10, search = "") => {
  return axiosClient.get("/Clients", {
    params: { pageNumber, pageSize, search },
  });
};

export const getClientById = (id) => {
  return axiosClient.get(`/Clients/${id}`);
};

export const createClient = (data) => {
  return axiosClient.post("/Clients", data);
};

export const updateClient = (id, data) => {
  return axiosClient.put(`/Clients/${id}`, data);
};

export const deleteClient = (id) => {
  return axiosClient.delete(`/Clients/${id}`);
};