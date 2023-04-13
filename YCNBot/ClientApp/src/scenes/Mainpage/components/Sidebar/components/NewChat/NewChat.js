import React from "react";
import classes from "./NewChat.module.css";
import AddIcon from '@mui/icons-material/Add';

const NewChat = () => {
    return (
        <a 
            className={classes.newChat}
            href="/chat" 
        >
            <AddIcon/>            
            <span className={classes.text}>
                New chat
            </span>
        </a>
    )
};

export default NewChat;