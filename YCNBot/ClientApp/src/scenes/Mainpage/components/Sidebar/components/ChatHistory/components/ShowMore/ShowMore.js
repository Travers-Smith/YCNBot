import { Button } from "@mui/material";
import classes from "./ShowMore.module.css";
import React from "react";

const ShowMore = ({ setPageNumber, hasMore }) => {
    return (
        <div
            className={classes.showMore}
        >
            <Button
                style={{
                    color: "white"
                }}
                disabled={!hasMore}
                onClick={() => setPageNumber(pageNumber => pageNumber + 1)} 
            >
                Show More
            </Button>
        </div>
    );
};

export default ShowMore;