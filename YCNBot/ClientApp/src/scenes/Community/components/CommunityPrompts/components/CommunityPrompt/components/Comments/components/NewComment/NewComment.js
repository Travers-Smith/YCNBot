import React, { useState } from "react";
import classes from "./NewComment.module.css"
import useSafeDataFetch from "../../../../../../../../../../hooks/useSafeDataFetch";
import { useContext } from "react";
import GlobalContext from "../../../../../../../../../../context/GlobalContext";

const NewComment = ({ communityPromptId, setComments, displayComments, setDisplayComments }) => { 
    const [comment, setComment] = useState("");
    const [{ isLoading }, safeDataFetch] = useSafeDataFetch();
    const { user } = useContext(GlobalContext)
    const add = async e => {
        e.preventDefault();
        
        const response = await safeDataFetch({
            data: {
                communityPromptId,
                comment
            },
            method: "POST",
            url: "/api/community-prompt-comment/add",
        });

        if(!response.isError){
            if(displayComments){
                console.log("hy")
                setComments(comments => [
                    {
                        user,
                        comment: comment,
                        dateAdded: new Date()
                    },
                    ...comments
                ]);
            }
            
            setDisplayComments(true)

            setComment("");
        }
    }

    return (
        <form 
            className={classes.newComment}
            onSubmit={add}    
        >
            <input
                placeholder="Add a comment..."
                value={comment}
                onChange={e => setComment(e.target.value)}
            />
        </form>
    )
};

export default  NewComment;