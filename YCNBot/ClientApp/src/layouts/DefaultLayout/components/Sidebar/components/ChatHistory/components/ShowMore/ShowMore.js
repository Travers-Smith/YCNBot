import { Button } from "@mui/material";
import classes from "./ShowMore.module.css";
import React, { useContext } from "react";
import GlobalContext from "../../../../../../../../context/GlobalContext";

const ShowMore = ({ setPageNumber, hasMore, dark }) => {
    const { darkMode } = useContext(GlobalContext);
    return (
        <div
            className={classes.showMore}
        >
            <Button
                className={(dark || darkMode) ? classes.dark : undefined}
                disabled={!hasMore}
                onClick={() => setPageNumber(pageNumber => pageNumber + 1)} 
            >
                Show More
            </Button>
        </div>
    );
};

export default ShowMore;