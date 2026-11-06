"use client";

import { useState } from "react";
import { useAuth } from "../_hooks/use-auth";

export default function Auth() {

    const { register, login, logout } = useAuth();

    // Inputs pour l'inscription
    const [regUsername, setRegUsername] = useState("");
    const [regEmail, setRegEmail] = useState("");
    const [regPass, setRegPass] = useState("");
    const [regPassCon, setRegPassCon] = useState("");

    // Inputs pour la connexion
    const [logUsername, setLogUsername] = useState("");
    const [logPass, setLogPass] = useState("");

    return (
        <div>
            <h3>Inscription</h3>
            <input type="text" placeholder="Pseudonyme" value={regUsername} onChange={(e) => setRegUsername(e.target.value)} />
            <input type="text" placeholder="Adresse courriel" value={regEmail} onChange={(e) => setRegEmail(e.target.value)} />
            <input type="text" placeholder="Mot de passe" value={regPass} onChange={(e) => setRegPass(e.target.value)} />
            <input type="text" placeholder="Confirmer le mot de passe" value={regPassCon} onChange={(e) => setRegPassCon(e.target.value)} />
            <button onClick={() => register(regUsername, regEmail, regPass, regPassCon)}>S'inscrire</button>

            <h3>Connexion</h3>
            <input type="text" placeholder="Pseudonyme" value={logUsername} onChange={(e) => setLogUsername(e.target.value)} />
            <input type="text" placeholder="Mot de passe" value={logPass} onChange={(e) => setLogPass(e.target.value)} />
            <button onClick={() => login(logUsername, logPass)}>Se connecter</button>

            <h3>Déconnexion</h3>
            <button onClick={() => logout()}>Se déconnecter</button>
        </div>
    );

}