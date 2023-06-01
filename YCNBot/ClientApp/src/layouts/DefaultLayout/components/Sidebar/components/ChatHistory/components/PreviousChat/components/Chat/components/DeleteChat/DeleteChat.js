import React from "react";
import classes from "./DeleteChat.module.css";
import { CircularProgress, IconButton } from "@mui/material";
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import useSafeDataFetch from "../../../../../../../../../../../../hooks/useSafeDataFetch";
import { useNavigate } from "react-router-dom";

const DeleteChat = ({ previousChatIdentifier, chat, setPreviousChats, setError }) => {
    const [{ isLoading }, safeDataFetch] = useSafeDataFetch();
    const navigate = useNavigate();

    const removeChat = async () => {
        const response = await safeDataFetch({
            url: "/api/chat/delete/" + previousChatIdentifier,
            method: "PATCH"
        });

        if(!response.isError){
            if(previousChatIdentifier === chat?.uniqueIdentifier){
                navigate("/");
            }

            setPreviousChats(chats => chats.filter(chat => chat.uniqueIdentifier !== previousChatIdentifier));
        } else {
            setError("Unable to delete chat");
        }
    };

    return (
        <IconButton
            onClick={removeChat}
        >
            {
                isLoading ?
                    <CircularProgress
                        className={classes.loading}
                        size={25}
                    />
                :
                    <DeleteOutlineIcon 
                        htmlColor="white"
                        fontSize="small"    
                    />
            }
        </IconButton>
    )

};

export default DeleteChat;