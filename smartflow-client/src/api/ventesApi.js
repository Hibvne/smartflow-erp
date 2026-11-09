import axiosClient from "./axiosClient";

export const getVentes = (pageNumber = 1, pageSize = 10) => {
  return axiosClient.get("/Ventes", { params: { pageNumber, pageSize } });
};

export const getVenteById = (id) => {
  return axiosClient.get(`/Ventes/${id}`);
};

export const createVente = (data) => {
  return axiosClient.post("/Ventes", data);
};