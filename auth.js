import { request } from "./api.js";

const login = async (email, password) => {
    const data = await request("/auth/login", {
        method: "POST",
        body: JSON.stringify({ email, password })
    });

    if (!data) return false;

    sessionStorage.setItem("accessToken", data.accessToken);
    return true;
};

const register = async (name, email, password) => {
    const data = await request("/auth/register", {
        method: "POST",
        body: JSON.stringify({ name, email, password })
    });

    return data !== null;
};

const logout = async () => {
    await request("/auth/revoke", {
        method: "POST",
        credentials: "include"
    });

    sessionStorage.removeItem("accessToken");
    window.location.href = "/pages/login.html";
};

export { login, register, logout };