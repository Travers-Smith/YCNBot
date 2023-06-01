import React, { useEffect, useState } from "react";
import { Navigate, Route, Routes, useNavigate } from "react-router-dom";
import GlobalContext from "./context/GlobalContext";
import useSafeDataFetch from "./hooks/useSafeDataFetch";
import Login from "./scenes/Login/Login";
import Chat from "./scenes/Chat/Chat";
import UserReport from "./scenes/UserReport/UserReport";
import UsageDashboard from "./scenes/UsageDashboard/UsageDashboard";
import DefaultLayout from "./layouts/DefaultLayout/DefaultLayout";
import AcceptTermsDialog from "./components/AcceptTermsDialog/AcceptTermsDialog";
import Community from "./scenes/Community/Community";

const YCNBot = () => {
    const [darkMode, setDarkMode] = useState(sessionStorage.getItem("darkMode") === "true");
    const safeDataFetch = useSafeDataFetch()[1];
    const navigate = useNavigate();
    const [checkedLoggedIn, setCheckedLoggedIn] = useState(false);
    const [user, setUser] = useState(null);
    const [previousChats, setPreviousChats] = useState([]);
    const [chat, setChat] = useState({
        messages: [],
        name: ""
    });

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

    const userAgreedToTerms = sessionStorage.getItem("agreedToTerms") === "true";

    return (
        <GlobalContext.Provider
            value={{
                darkMode,
                setDarkMode,
                user,
                setUser
            }}
        >
            {
                user && 
                    <AcceptTermsDialog
                        open={!userAgreedToTerms}
                        setUserAgreedToTerms={agreed => sessionStorage.setItem("agreedToTerms", agreed)}
                    />
            }
                    <Routes >
                        <Route path="/login" element={<Login/>}/>
                        {
                            (user && userAgreedToTerms) &&
                                <Route 
                                    element={
                                        <DefaultLayout 
                                            chat={chat}
                                            previousChats={previousChats}
                                            setPreviousChats={setPreviousChats}
                                        />
                                    }
                                >
                                    {
                                        checkedLoggedIn &&
                                            <Route path="/" element={<Navigate to="/chat" replace/>}/>
                                    }
                                    <Route path="/">
                                        <Route 
                                            path="/chat" 
                                            element={
                                                <Chat 
                                                    chat={chat}
                                                    setChat={setChat}
                                                    setPreviousChats={setPreviousChats}
                                                />
                                            }
                                        />
                                        <Route 
                                            path="/chat/:chatIdentifier"     
                                            element={
                                                <Chat 
                                                    chat={chat}
                                                    setChat={setChat}
                                                    setPreviousChats={setPreviousChats}
                                                />
                                            }
                                        />
                                        {
                                            user?.isAdmin &&
                                                <Route path="user-usage" element={<UserReport/>}/>
                                        }
                                    </Route>
                                    <Route path="/community" element={<Community/>}/>
                                    <Route path="/usage" element={<UsageDashboard/>}/>
                            </Route>
                        }
                    </Routes>
        </GlobalContext.Provider>            
    )
};

export default YCNBot;