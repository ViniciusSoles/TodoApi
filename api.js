
const BASE_URL = "http://localhost:5291/api";

// pega o token guardado
const getToken = () => sessionStorage.getItem("accessToken");

// requisição genérica
const request = async (endpoint, options = {}) => {
    const token = getToken();

    const headers = {
        "Content-Type": "application/json",
        ...(token && { "Authorization": `Bearer ${token}` })
    };

    const response = await fetch(`${BASE_URL}${endpoint}`, {
        ...options,
        headers,
        credentials: "include" // ← envia o cookie do refresh token
    });

    if (response.status === 401) {
        // tenta renovar o token
        const renewed = await refreshToken();
        if (!renewed) {
            window.location.href = "/pages/login.html";
            return null;
        }
        // tenta a requisição de novo com o novo token
        return request(endpoint, options);
    }

    if (response.status === 204)
        return null; // sem corpo

    return response.json();
};

// renova o access token
const refreshToken = async () => {
    const response = await fetch(`${BASE_URL}/auth/refresh`, {
        method: "POST",
        credentials: "include" // ← manda o cookie automaticamente
    });

    if (!response.ok) {
        sessionStorage.removeItem("accessToken");
        return false;
    }

    const data = await response.json();
    sessionStorage.setItem("accessToken", data.accessToken);
    return true;
};

export { request, getToken };