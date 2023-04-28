import React, { useState } from "react";
import { 
    Button, 
    Dialog, 
    DialogActions, 
    DialogContent, 
    DialogContentText, 
    DialogTitle } from "@mui/material";
import config from "../../../../../../config.json";

const RecordChat = ({ show, setShow, setRecordChat }) => {
    const [showPersonalModeWarning, setShowPersonalModeWarning] = useState(false);

    const updateRecordChat = recordChat => {
        setRecordChat(recordChat);
        setShow(false);
        setShowPersonalModeWarning(false);
    }

    return (
        <Dialog 
            open={show} 
        >
            <DialogTitle>Record Chat</DialogTitle>
            <DialogContent>
            <DialogContentText>
                What are you using this chat for? All work chats will be recorded
            </DialogContentText>
            <br/>
            {
                showPersonalModeWarning &&
                    <DialogContentText
                        color="red"
                    >
                        Due to ongoing concerns with privacy regulators we have temporarily blocked personal use of  {config.APP_NAME}. 
                        We’ll re-enable this as and when the privacy regulators update on their position.
                    </DialogContentText>
            }
            </DialogContent>
            <DialogActions>
                <Button onClick={() => config.ALLOW_PERSONAL_MODE ? updateRecordChat(false) : setShowPersonalModeWarning(true)}>
                    Personal
                </Button>
                <Button onClick={() => updateRecordChat(true)}>
                    Work
                </Button>
            </DialogActions>
        </Dialog>
    )
};

export default RecordChat;