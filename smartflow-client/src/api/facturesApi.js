import axiosClient from "./axiosClient";

export const getFactures = (pageNumber = 1, pageSize = 10) => {
  return axiosClient.get("/Factures", { params: { pageNumber, pageSize } });
};

export const enregistrerPaiement = (id, data) => {
  return axiosClient.post(`/Factures/${id}/paiement`, data);
};