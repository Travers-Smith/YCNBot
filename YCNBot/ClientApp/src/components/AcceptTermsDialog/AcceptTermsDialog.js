import React, { useContext } from "react";
import useSafeDataFetch from "../../hooks/useSafeDataFetch";
import GlobalContext from "../../context/GlobalContext";
import { 
    Button, 
    CircularProgress, 
    Dialog, 
    DialogActions,
    DialogContent, 
    DialogContentText, 
    DialogTitle } from "@mui/material";
import config from "../../config.json";

const AcceptTermsDialog = ({ open, setUserAgreedToTerms }) => {
    const [{ isLoading, isError }, safeFetch] = useSafeDataFetch();
    
    const {
        setUser
    } = useContext(GlobalContext);

    const agreeToTerms = async () => {
        const response = await safeFetch({
            url: "/api/user-terms/accept-terms",
            method: "POST"
        });

        setUserAgreedToTerms(true)

        if(!response.isError) {
            setUser(user => ({
                ...user,
                agreedToTerms: true
            }));            
        }
    };

    return ( 
        <Dialog 
            open={open} 
        >
            <DialogTitle>Accept Terms</DialogTitle>
            <DialogContent>
                <div>
                    <h6>Input</h6>
                    <ul>
                        <DialogContentText>
                            {
                                config.COMPANY_INPUT_TERMS.map((term, index) => (
                                    <li key={index}>{term}</li>
                                ))
                            }
                        </DialogContentText>
                    </ul>
                </div>
                <div>
                    <h6>Output</h6>
                    <ul>
                        <DialogContentText>
                            {
                                config.COMPANY_OUTPUT_TERMS.map((term, index) => (
                                    <li key={index}>{term}</li>
                                ))
                            }
                        </DialogContentText>
                    </ul>
                </div>
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