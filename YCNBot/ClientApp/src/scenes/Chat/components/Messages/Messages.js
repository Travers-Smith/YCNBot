import React, { useEffect, useRef } from "react";
import classes from "./Messages.module.css";
import Message from "./components/Message/Message";

const Messages = ({ messages, recordChat }) => {
    const messagesRef = useRef();

    useEffect(() => {
        messagesRef.current.scrollTo({
            top: messagesRef?.current.scrollHeight,
            behaviour: "smooth"
        })
    }, [messages]);

    return (
        <div 
            className={classes.messages}
            ref={messagesRef}    
        >
            {
                messages?.map((message, index) => (
                    <Message
                        key={message.uniqueIdentifier ?? index}
                        data={message}
                        questionText={(message.isSystem && index > 0) ? messages[index - 1].text : null}
                        recordChat={recordChat}
                    />
                ))             
            }
        </div>
    )
};

export default Messages;