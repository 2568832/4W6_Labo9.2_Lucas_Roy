import axios from "axios";

export const stimeRequest = axios.create();

stimeRequest.interceptors.request.use((config) => {

    config.headers["Content-Type"] = "application/json";
    config.headers.Authorization = "Bearer " + localStorage.getItem("token");

    return config;

});