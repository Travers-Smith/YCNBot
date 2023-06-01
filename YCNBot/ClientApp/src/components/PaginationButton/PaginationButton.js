import React, { useContext } from "react";
import classes from "./PaginationButton.module.css";
import { Button, CircularProgress } from "@mui/material";
import GlobalContext from "../../context/GlobalContext";

const PaginationButton = ({ hasMore, isLoading, setPageNumber }) => {
    const {
        darkMode
    } = useContext(GlobalContext);

    return (
        <div className={classes.showMoreContainer}>
            <Button 
                className={classes.showMore}
                style={darkMode ? {
                    color: "white"
                } : undefined}
                disabled={!hasMore}
                onClick={() => setPageNumber(pn => pn + 1)}
            >
                {
                    isLoading ?
                        <CircularProgress/>
                    :
                        "Show More"
                }
            </Button>
        </div>
    )
};

export default PaginationButton;