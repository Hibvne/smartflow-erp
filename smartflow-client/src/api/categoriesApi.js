import axiosClient from "./axiosClient";

export const getCategories = () => {
  return axiosClient.get("/Categories");
};

export const createCategorie = (data) => {
  return axiosClient.post("/Categories", data);
};

export const updateCategorie = (id, data) => {
  return axiosClient.put(`/Categories/${id}`, data);
};

export const deleteCategorie = (id) => {
  return axiosClient.delete(`/Categories/${id}`);
};