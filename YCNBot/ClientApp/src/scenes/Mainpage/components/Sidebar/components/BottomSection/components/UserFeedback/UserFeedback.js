import React, { useState } from "react";
import classes from "./UserFeedback.module.css";
import { 
    Button,
    CircularProgress, 
    Dialog, 
    DialogActions, 
    DialogContent, 
    DialogContentText, 
    DialogTitle, 
    FormControl, 
    InputLabel, 
    MenuItem, 
    Select, 
    TextField} from "@mui/material";
import useSafeDataFetch from "../../../../../../../../hooks/useSafeDataFetch";
import FeedbackIcon from '@mui/icons-material/Feedback';
import ActionButton from "../ActionButton/ActionButton";

const UserFeedback = () => {
    const [{ isLoading, isError}, safeDataFetch] = useSafeDataFetch();
    const [open, setOpen] = useState(false);
    const [text, setText] = useState("");
    const [feedbackTypeId, setFeedbackTypeId] = useState(1);

    const submitFeedback = async () => {
        const response = safeDataFetch({
            url: "/api/user-feedback/add",
            data: {
                text: text,
                feedbackTypeId: feedbackTypeId
            },
            method: "POST"
        });

        if(!response.isError){
            setOpen(false);
        }
    }
    return (
        <>
            <ActionButton
                icon={<FeedbackIcon/>}
                onClick={() => setOpen(true)}
            >
                Submit Feedback
            </ActionButton>
            <Dialog 
                open={open} 
            >
                <DialogTitle>Feedback</DialogTitle>
                <DialogContent>
                    <div className={classes.container}>
                        <TextField
                            value={text}
                            onChange={e => setText(e.target.value)}
                            minRows={4}
                            multiline
                            label="Feedback"
                        />
                        <FormControl fullWidth>
                            <InputLabel id="demo-simple-select-label">Feature Type</InputLabel>
                            <Select
                                value={feedbackTypeId}
                                label="Feature Type"
                                onChange={e => setFeedbackTypeId(e.target.value)}
                            >
                                <MenuItem value={1}>Issue</MenuItem>
                                <MenuItem value={2}>Feature</MenuItem>
                            </Select>
                        </FormControl>
                    </div>
                {
                    isError &&
                        <>
                            <br/>
                            <DialogContentText
                                color="red"
                            >
                                Unable to submit feedback.
                            </DialogContentText>
                        </>
                }
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpen(false)}>
                        Close
                    </Button>
                    <Button onClick={submitFeedback}>
                        {
                            isLoading ?
                                <CircularProgress />
                            :
                                "Agree"
                        }
                    </Button>
                </DialogActions>
            </Dialog>
        </>
    )
};

export default UserFeedback;