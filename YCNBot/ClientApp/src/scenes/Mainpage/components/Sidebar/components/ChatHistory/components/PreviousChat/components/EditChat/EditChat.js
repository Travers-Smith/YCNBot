import React, { useState } from "react";
import classes from "./EditChat.module.css";
import { CircularProgress, IconButton } from "@mui/material";
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import CancelIcon from '@mui/icons-material/Cancel';
import useSafeDataFetch from "../../../../../../../../../../hooks/useSafeDataFetch";
import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';

const EditChat = ({ chat, setPreviousChats, setEdit, setError }) => {
    const [{ isLoading }, safeDataFetch] = useSafeDataFetch();
    
    const [newChatName, setNewChatName] = useState(chat?.name);

    const editChatName = async () => {
        const response = await safeDataFetch({
            data: {
                chatIdentifier: chat.uniqueIdentifier,
                name: newChatName
            },
            method: "PATCH",
            url: "/api/chat/update"
        });

        if(!response.isError){
            setPreviousChats(previousChats => previousChats.map(previousChat => {
                if(previousChat.uniqueIdentifier === chat.uniqueIdentifier){
                    previousChat.name = newChatName
                }
    
                return previousChat
            }));
        } else {
            setError("Unable to edit chat");
        }

        setEdit(false);
    };

    return (
        <div className={classes.edit}>
            <ChatBubbleOutlineIcon
                    fontSize="small"
                />
            <input 
                onChange={e => setNewChatName(e.target.value)} 
                maxLength={100}
                value={newChatName}    
            />
            <div className={classes.editChatButtonsContainer}>
                <IconButton
                    onClick={editChatName}
                >
                    {
                        isLoading ?
                            <CircularProgress
                                className={classes.loading}
                                size={25}
                            />
                        :
                            <CheckCircleIcon
                                htmlColor="#B3B3B8"
                            />
                    }
                </IconButton>
                <IconButton
                    onClick={() => setEdit(false)}
                >
                    <CancelIcon
                        htmlColor="#B3B3B8"
                    />
                </IconButton>
            </div>
        </div>
    )
};

export default EditChat;
