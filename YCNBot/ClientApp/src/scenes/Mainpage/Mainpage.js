import React, { useContext, useEffect, useState } from "react";
import classes from "./Mainpage.module.css";
import Chat from "./components/Chat/Chat";
import Sidebar from "./components/Sidebar/Sidebar";
import GlobalContext from "../../context/GlobalContext";
import AcceptTermsDialog from "./components/AcceptTermsDialog/AcceptTermsDialog";

const Mainpage = () => {
    const [open, setOpen] = useState(false);
    const [previousChats, setPreviousChats] = useState([]);
    const [chat, setChat] = useState({
        messages: [],
        name: ""
    });

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
                open={open}
                setOpen={setOpen}
            />
            {
                user?.agreedToTerms &&
                    <>
                        <Sidebar
                            chat={chat}
                            previousChats={previousChats}
                            setPreviousChats={setPreviousChats}
                        />
                        <Chat
                            chat={chat}
                            setChat={setChat}
                            setPreviousChats={setPreviousChats}
                        />
                    </>
            }
        </div>
    )
};

export default Mainpage;