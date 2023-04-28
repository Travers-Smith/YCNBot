import React, { useContext } from "react";
import GlobalContext from "../../../../../../context/GlobalContext";
import classes from "./NewMessage.module.css";
import SendIcon from '@mui/icons-material/Send';
import { CircularProgress, IconButton } from "@mui/material";
import useSafeDataFetch from "../../../../../../hooks/useSafeDataFetch";
import config from "../../../../../../config.json";

const NewMessage = ({ 
    chat, 
    setChat, 
    newMessageRef, 
    recordChat,
    setPreviousChats 
}) => {

    const [{ isLoading }, safeRequest] = useSafeDataFetch();

    const sendPendingMessage = messageText => {
        setChat(chat => ({
            ...chat,
            messages: [
                ...chat.messages,
                {
                    text: messageText,
                    isSystem: false
                },
                {
                    pending: true,
                    isSystem: true
                }
            ]
        }));

        newMessageRef.current.innerText = "";
    }
    
    const addNonRecordedMessage = async newMessageText => {
        if(!config.ALLOW_PERSONAL_MODE){
            return;
        }
        const oldMessages = [...chat.messages];

        sendPendingMessage(newMessageText)
        
        const response = await safeRequest({
            url: "/api/message/add-non-recorded-message",
            method: "POST",
            data: [ 
                ...oldMessages,
                {
                    text: newMessageText,
                    isSystem: false
                }
            ]
        });

        const data = response.data;

        setChat(chat => ({
            ...chat,
            messages: [
                ...chat.messages.filter(message => !message.pending),
                {
                    text: !response.isError && data.text,
                    isSystem: true,
                    isError: response.isError
                }
            ]
        }));

    }

    const addRecordedMessage = async newMessageText => {
        sendPendingMessage(newMessageText)

        const response = await safeRequest({
            url: "/api/message/add-recorded-message",
            method: "POST",
            data: {
                chatIdentifier: chat?.uniqueIdentifier,
                message: newMessageText
            }
        });
        
        const data = response.data;
        
        if(!response.isError){
            if(!chat.uniqueIdentifier){
                setPreviousChats(previousChats => [
                    {
                        name: data.chat.name,
                        uniqueIdentifier: data.chat.uniqueIdentifier
                    },
                    ...previousChats
                ])
            }
            
            setChat(chat => ({
                name: data.chat.name,
                uniqueIdentifier: data.chat.uniqueIdentifier,
                messages: [
                    ...chat.messages.filter(message => !message.pending),
                    {
                        text: data.text,
                        isSystem: true,
                        uniqueIdentifier: data.uniqueIdentifier
                    }
                ]
            }));
        } else {
            setChat(chat => ({
                ...chat,
                messages: [
                    ...chat.messages.filter(message => !message.pending),
                    {
                        isSystem: true,
                        isError: true,
                        isPiError: response.errorMessage === "contains personal information"
                    }
                ]
            }))
        }
    }

    const addMessage = () => {
        const newMessageText = newMessageRef.current.innerText;

        if(!newMessageText){
            return;
        }

        if(recordChat) {
            addRecordedMessage(newMessageText)
        } else {
            addNonRecordedMessage(newMessageText);
        }
    }

    const submitOnEnter = e => {
        if (e.keyCode === 13){
            e.preventDefault();
            addMessage();
        }
    }
    
    const {
        darkMode
    } = useContext(GlobalContext);

    return (
        <div>
            <div className={classes.newMessageContainer}>
                <div className={classes.newMessageBody}>
                    <div
                        onKeyDown={submitOnEnter}
                        className={`${classes.newMessage} ${darkMode ? classes.dark : classes.light}`}
                        contentEditable
                        ref={newMessageRef}
                    >
                        
                    </div>
                    <div className={classes.submitButton}>
                        {
                            isLoading ?
                                <div
                                    className={classes.loadingContainer}                                
                                >
                                    <CircularProgress
                                        size={25}
                                        className={classes.loading}
                                    />
                                </div>
                            :
                                <IconButton 
                                    onClick={addMessage}
                                >
                                    <SendIcon
                                        htmlColor={darkMode ? "#FFF" : "#666670"}
                                    />
                                </IconButton>
                        }
                    </div>
                </div>
        </div>
        {
            !recordChat &&
                <p className={`${classes.recordChatWarning} ${darkMode ? classes.dark : classes.light}`}>
                    This chat is not being recorded
                </p>
        }
        </div>
    )
};

export default NewMessage;