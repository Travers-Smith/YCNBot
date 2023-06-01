import React, { useState } from "react";
import classes from "./PreviousChat.module.css";
import EditChat from "./components/EditChat/EditChat";
import Chat from "./components/Chat/Chat";

const PreviousChat = ({ chat, data, setPreviousChats }) => {
    const {
        uniqueIdentifier: previousChatIdentifier,
    } = data;

    const [error, setError] = useState("");
    const [edit, setEdit] = useState(false);
    
    return (
        <div className={classes.previousChatContainer}>
            <div
                className={classes.previousChat}
            >
                {
                    edit ?
                        <EditChat
                            chat={data}
                            setPreviousChats={setPreviousChats}
                            setEdit={setEdit}
                            setError={setError}
                        />
                    :
                        <Chat
                            chat={chat}
                            data={data}
                            edit={edit}
                            setEdit={setEdit}
                            previousChatIdentifier={previousChatIdentifier}
                            setPreviousChats={setPreviousChats}
                            error={error}
                            setError={setError}
                        />
                }
            </div>
            {
                error && 
                    <p className={classes.error}>{error}</p>
            }
        </div>
    )
};

export default PreviousChat;