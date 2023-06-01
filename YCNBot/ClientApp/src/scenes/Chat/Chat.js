import React, { useContext, useEffect, useRef, useState } from "react";
import classes from "./Chat.module.css";
import Messages from "./components/Messages/Messages";
import NewMessage from "./components/NewMessage/NewMessage";
import useSafeDataFetch from "../../hooks/useSafeDataFetch";
import { useParams } from "react-router-dom";
import NewChatStarterPage from "./components/NewChatStarterPage/NewChatStarterPage";
import RecordChat from "./components/RecordChat/RecordChat";
import { CircularProgress } from "@mui/material";
import ReportProblemIcon from '@mui/icons-material/ReportProblem';
import GlobalContext from "../../context/GlobalContext";

const Chat = ({ chat, setChat, setPreviousChats }) => {
    const {
        darkMode
    } = useContext(GlobalContext);

    const { chatIdentifier } = useParams();
    const [recordChat, setRecordChat] = useState(true);
    const [showRecordChat, setShowRecordChat] = useState(false);

    const newMessageRef = useRef();
    
    const [{ isLoading, isError }, fetchData] = useSafeDataFetch();

    useEffect(() => {
        if(!chatIdentifier){
            setShowRecordChat(true)
        }

        const fetchChat = async () => {
            const response = await fetchData({
                url: "/api/chat/get-by-unique-identifier/" + chatIdentifier
            });

            if(!response.isError){
                setChat(response.data);
            }
        }

        if(chatIdentifier){
            fetchChat();
        }
        else {
            setChat({
                messages: [],
                name: ""
            })
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [chatIdentifier])

    return (
        <div className={`${classes.chat} ${darkMode ? classes.dark : classes.light}`}>
            <RecordChat
                show={showRecordChat}
                setShow={setShowRecordChat}
                recordChat={recordChat}
                setRecordChat={setRecordChat}
            />
            {
                isLoading ?
                    <div className={classes.loadingContainer}>
                        <CircularProgress
                            className={darkMode ? classes.darkProgress : classes.lightProgress}
                        />
                    </div>
                : isError ?
                    <div className={classes.error}>
                        <ReportProblemIcon
                            htmlColor="red"
                        />
                        <div>
                            Sorry, we were unable to retrieve the required data. 
                            
                            Please try again later or contact our support team for assistance.
                        </div>
                    </div>
                : chat.messages?.length > 0 ?
                    <Messages
                        messages={chat.messages}
                        setChat={setChat}       
                        recordChat={recordChat}
                    />
                :
                    <NewChatStarterPage
                        newMessageRef={newMessageRef}
                    />
            }
            <NewMessage
                chat={chat}
                setChat={setChat}
                newMessageRef={newMessageRef}
                recordChat={recordChat}
                setPreviousChats={setPreviousChats}
            />
        </div>
    )
}

export default Chat;