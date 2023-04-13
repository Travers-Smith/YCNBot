import React, { useState } from "react";
import classes from "./MessageRating.module.css";
import { Rating } from "@mui/material";
import useSafeDataFetch from "../../../../../../../../../../hooks/useSafeDataFetch";
import ReportProblemIcon from '@mui/icons-material/ReportProblem';
import StarIcon from '@mui/icons-material/Star';

const MessageRating = ({ messageIdentifier, currentRating }) => {
    const [rating, setRating] = useState(currentRating ?? 0);
    
    const [{ isError }, safeDataFetch] = useSafeDataFetch();

    const changeMessageRating = async (e, newValue) => {
        const response = await safeDataFetch({
            method: "PATCH",
            data: {
                messageIdentifier: messageIdentifier,
                rating: newValue
            },
            url: "/api/message/change-rating"
        });

        if(!response.isError){
            setRating(newValue)            
        }
    }
    
    return (
        <div className={classes.messageRating}>
            <div>How happy are you with the response?</div>
            <Rating
                value={rating}
                onChange={changeMessageRating}
                emptyIcon={<StarIcon style={{ color: "rgb(169, 168, 168)"}} fontSize="inherit" />}
            />
            {
                isError &&
                    <div className={classes.error}>
                        <ReportProblemIcon/>
                        <p>
                            Unable to add rating
                        </p>
                    </div>
            }
        </div>
    )
};

export default MessageRating;