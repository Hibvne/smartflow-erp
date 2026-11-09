import axiosClient from "./axiosClient";

export const getDashboardStats = () => {
  return axiosClient.get("/Dashboard/stats");
};