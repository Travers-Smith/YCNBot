import React from "react";
import classes from "./AddToCommunity.module.css";
import useSafeDataFetch from "../../../../../../../../hooks/useSafeDataFetch";
import { Button, CircularProgress } from "@mui/material";
import { useState } from "react";
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import { useContext } from "react";
import GlobalContext from "../../../../../../../../context/GlobalContext";

const AddToCommunity = ({ answerText, questionText }) => {
    const [{ isLoading, isError, errorMessage }, safeDataFetch] = useSafeDataFetch();
    const [addedToCommunity, setAddedToCommunity] = useState(false);

    const {
        darkMode
    } = useContext(GlobalContext);

    const add = async () => {
        const response = await safeDataFetch({
            data: {
                answer: answerText,
                question: questionText
            },
            method: "POST",
            url: "/api/community-prompt/add"
        });

        if(!response.isError){
            setAddedToCommunity(true);
        }
    }

    return (
        <div className={classes.addToCommunityContainer}>
            {
                addedToCommunity ?
                    <div className={classes.addedToCommunity}>
                        <CheckCircleIcon
                            htmlColor="green"
                        />
                        <div>Added to community</div>
                    </div>
                :
                    answerText &&
                        <>
                            <Button
                                className={darkMode && classes.darkButton}
                                onClick={add}
                            >
                                {
                                    isLoading ?
                                        <CircularProgress
                                            className={classes.loading}
                                        />
                                    : 
                                        "Promote to community"
                                }
                            </Button>
                            {
                                isError &&
                                    <p className={classes.error}>
                                        {errorMessage ?? "Unable to promote to community"}
                                    </p>
                            }
                        </>
            }
        </div>
    )
};

export default AddToCommunity;