import React, { useContext, useEffect, useState } from "react";
import classes from "./Mainpage.module.css";
import Chat from "./components/Chat/Chat";
import Sidebar from "./components/Sidebar/Sidebar";
import GlobalContext from "../../context/GlobalContext";
import AcceptTermsDialog from "./components/AcceptTermsDialog/AcceptTermsDialog";
import UserReport from "./components/UserReport/UserReport";

const Mainpage = () => {
    const [open, setOpen] = useState(false);
    const [previousChats, setPreviousChats] = useState([]);
    const [chat, setChat] = useState({
        messages: [],
        name: ""
    });
    const userAgreedToTerms = sessionStorage.getItem("agreedToTerms") === "true";
    const [showUserReport, setShowUserReport] = useState(false);
    const {
        user
    } = useContext(GlobalContext);

    useEffect(() => {
        if(user && !user?.agreedToTerms){
            setOpen(true);
        }
    }, [user]);

    return (
        <div className={classes.chat}>
            <AcceptTermsDialog
                open={!userAgreedToTerms}
                setUserAgreedToTerms={agreed => sessionStorage.setItem("agreedToTerms", agreed)}
            />
            {
                userAgreedToTerms &&
                    <>
                        <Sidebar
                            chat={chat}
                            previousChats={previousChats}
                            setPreviousChats={setPreviousChats}
                            setShowUserReport={setShowUserReport}
                        />
                        {
                            showUserReport ?    
                                <UserReport/>
                            :
                                <Chat
                                    chat={chat}
                                    setChat={setChat}
                                    setPreviousChats={setPreviousChats}
                                />
                        }
                    </>
            }
        </div>
    )
};

export default Mainpage;