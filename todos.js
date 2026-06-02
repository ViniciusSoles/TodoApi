import { request } from "./api.js";

const getAll = async () => {
    return await request("/todos");
};

const create = async (title, description) => {
    return await request("/todos", {
        method: "POST",
        body: JSON.stringify({ title, description })
    });
};

const update = async (id, title, description) => {
    return await request(`/todos/${id}`, {
        method: "PUT",
        body: JSON.stringify({ title, description })
    });
};

const complete = async (id) => {
    return await request(`/todos/${id}/complete`, {
        method: "PATCH"
    });
};

const remove = async (id) => {
    return await request(`/todos/${id}`, {
        method: "DELETE"
    });
};

export { getAll, create, update, complete, remove };