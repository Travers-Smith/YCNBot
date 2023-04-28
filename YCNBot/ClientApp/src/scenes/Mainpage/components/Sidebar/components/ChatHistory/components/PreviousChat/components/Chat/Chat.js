import React from "react";
import classes from "./Chat.module.css";
import { IconButton } from "@mui/material";
import EditIcon from '@mui/icons-material/Edit';
import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';
import { Link } from "react-router-dom";
import DeleteChat from "./components/DeleteChat/DeleteChat";

const Chat = ({ 
    chat,
    data, 
    edit, 
    setEdit, 
    previousChatIdentifier, 
    setPreviousChats,
    setError
}) => {
    return (
        <>
            <a href={"/chat/" + previousChatIdentifier}>
                <ChatBubbleOutlineIcon
                    fontSize="small"
                />
                <div className={classes.text}>
                    {data.name}
                </div>
            </a>
            {
                !edit &&
                    <div className={classes.rightSide}>
                        <IconButton
                            onClick={() => setEdit(true)}
                        >
                            <EditIcon 
                                htmlColor="white"
                                fontSize="small"    
                            />
                        </IconButton>
                        <DeleteChat
                            previousChatIdentifier={data.uniqueIdentifier}
                            chat={chat}
                            setPreviousChats={setPreviousChats}
                            setError={setError}
                        />
                    </div>
            }
        </>
    )
};

export default Chat;