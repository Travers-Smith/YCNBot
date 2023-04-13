import React, { useContext } from "react";
import useSafeDataFetch from "../../../../hooks/useSafeDataFetch";
import GlobalContext from "../../../../context/GlobalContext";
import { 
    Button, 
    CircularProgress, 
    Dialog, 
    DialogActions,
    DialogContent, 
    DialogContentText, 
    DialogTitle } from "@mui/material";
import config from "../../../../config.json";

const AcceptTermsDialog = ({ open, setOpen }) => {
    const [{ isLoading, isError }, safeFetch] = useSafeDataFetch();
    
    const {
        setUser
    } = useContext(GlobalContext);

    const agreeToTerms = async () => {
        const response = await safeFetch({
            url: "/api/user-terms/accept-terms",
            method: "POST"
        });

        if(!response.isError) {
            setUser(user => ({
                ...user,
                agreedToTerms: true
            }));
            
            setOpen(false);
        }
    };

    return ( 
        <Dialog 
            open={open} 
        >
            <DialogTitle>Accept Terms</DialogTitle>
            <DialogContent>
                <ul>
                    <DialogContentText>
                        {
                            config.COMPANY_TERMS?.map((term, index) => (
                                <li key={index}>{term}</li>
                            ))
                        }
                    </DialogContentText>
                </ul>
            {
                isError &&
                    <>
                        <br/>
                        <DialogContentText
                            color="red"
                        >
                            Unable to accept terms.
                        </DialogContentText>
                    </>
            }
            </DialogContent>
            <DialogActions>
                <Button onClick={agreeToTerms}>
                    {
                        isLoading ?
                            <CircularProgress />
                        :
                            "Agree"
                    }
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default AcceptTermsDialog;