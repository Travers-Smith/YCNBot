import React from "react";
import classes from "./BottomActionBar.module.css"
import Comments from "./components/Comments/Comments";
import Like from "./components/Like/Like";
import Likes from "./components/Likes/Likes";
import { useState } from "react";

const BottomActionBar = ({ communityPrompt, setCommunityPrompts, setDisplayComments }) => {
    const {
        id,
        likesCount, 
        commentsCount
    } = communityPrompt;

    const [userLikes, setUserLikes] = useState([]);
    
    return (
        <div className={classes.bottomActionBar}>
            <Like
                communityPrompt={communityPrompt}
                setCommunityPrompts={setCommunityPrompts}
                setUserLikes={setUserLikes}
            />
            <div className={classes.rightBar}>
                <Likes
                    communityPromptId={id}
                    likesCount={likesCount}
                    userLikes={userLikes}
                    setUserLikes={setUserLikes}
                />
                <Comments
                    commentCount={commentsCount}
                    setDisplayComments={setDisplayComments}
                />
            </div>
        </div>
    );
};

export default BottomActionBar;