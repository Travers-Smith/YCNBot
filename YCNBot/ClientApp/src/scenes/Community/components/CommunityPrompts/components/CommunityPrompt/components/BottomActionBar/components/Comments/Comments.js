import React from "react";
import { Button } from "@mui/material";

const Comments = ({ commentCount, setDisplayComments }) => {
    return (
        <>
            {
                commentCount > 0 &&
                    <Button
                        onClick={() => setDisplayComments(true)}
                    >
                        {commentCount} Comments
                    </Button>
            }
        </>
    );
}

export default Comments;