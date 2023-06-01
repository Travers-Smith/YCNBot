import React from "react";
import classes from "./Like.module.css";
import { Button, CircularProgress } from "@mui/material";
import ThumbUpIcon from '@mui/icons-material/ThumbUp';
import ThumbUpOffAltIcon from '@mui/icons-material/ThumbUpOffAlt';
import useSafeDataFetch from "../../../../../../../../../../hooks/useSafeDataFetch";
import { useContext } from "react";
import GlobalContext from "../../../../../../../../../../context/GlobalContext";

const Like = ({ communityPrompt, setCommunityPrompts, setUserLikes }) => {
    const [{ isLoading }, safeDataFetch] = useSafeDataFetch();

    const {
        userLiked,
        id: communityPromptId
    } = communityPrompt;

    const { user } = useContext(GlobalContext);

    const updateLike = async () => {
        const response = await safeDataFetch({
            data: {
                communityPromptId: communityPromptId,
                liked: !userLiked
            },
            method: "PATCH",
            url: "/api/community-prompt-like/update"
        })

        if(!response.isError){
            if(!userLiked){
                setUserLikes(userLikes => {
                    return [...userLikes, user];
                })
            } else {
                setUserLikes(userLikes => userLikes.filter(ul => ul.email ===  user.email));
            }
            setCommunityPrompts(cps => cps.map(cp => {
                if(cp.id === communityPromptId){
                    cp.userLiked = !userLiked

                    if(!userLiked){
                        cp.likesCount = cp.likesCount + 1;
                    } else {
                        cp.likesCount = cp.likesCount - 1;
                    }
                }

                return cp;
            }))
        }
    }

    return (
        <Button
            onClick={updateLike}
        >
            {
                isLoading ? 
                    <CircularProgress/>
                :
                    <>
                        {
                            userLiked ?
                                <ThumbUpIcon/>
                            :
                                <ThumbUpOffAltIcon/>    
                        }
                        <span className={classes.likeText}>
                            Like
                        </span>
                    </>
            }
        </Button>
    )
};

export default Like;