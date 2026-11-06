import axios from "axios";

const domain = "https://localhost:7013/";

export function useAuth() {

    async function register(username: string, email: string, password: string, passwordConfirm: string) {

        const registerDTO = {
            username: username,
            email: email,
            password: password,
            passwordConfirm: passwordConfirm
        };

        const x = await axios.post(domain + "api/Users/Register", registerDTO);
        console.log(x.data);

    }

    async function login(username: string, password: string) {

        const loginDTO = {
            username: username,
            password: password
        };

        const x = await axios.post(domain + "api/Users/Login", loginDTO);
        console.log(x.data);
        localStorage.setItem("token", x.data.token);

    }

    function logout() {

        localStorage.removeItem("token");

    }

    return { register, login, logout };

}