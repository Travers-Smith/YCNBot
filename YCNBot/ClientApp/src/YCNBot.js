import React, { useEffect, useState } from "react";
import { Navigate, Route, Routes, useNavigate } from "react-router-dom";
import GlobalContext from "./context/GlobalContext";
import useSafeDataFetch from "./hooks/useSafeDataFetch";
import Login from "./scenes/Login/Login";
import Mainpage from "./scenes/Mainpage/Mainpage";

const LawBot = () => {
    const [darkMode, setDarkMode] = useState(sessionStorage.getItem("darkMode") === "true");
    const safeDataFetch = useSafeDataFetch()[1];
    const [checkedLoggedIn, setCheckedLoggedIn] = useState(false);
    const [user, setUser] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            const response = await safeDataFetch({
                url: "/api/identity/get-user"
            });

            if(!response.data){
                navigate("/login")
            } else {
                setUser(response.data)
            }

            setCheckedLoggedIn(true);
        };

        fetchData();
    }, [])

    return (
        <GlobalContext.Provider
            value={{
                darkMode,
                setDarkMode,
                user,
                setUser
            }}
        >
            <Routes>
                <Route path="/login" element={<Login/>}/>
                {
                    checkedLoggedIn &&
                        <Route path="/" element={<Navigate to="/chat" replace/>}/>
                }
                <Route path="/chat">
                    <Route path="" element={<Mainpage/>}/>
                    <Route path=":chatIdentifier" element={<Mainpage/>}/>
                </Route>
            </Routes>
        </GlobalContext.Provider>
    )
};

export default LawBot;