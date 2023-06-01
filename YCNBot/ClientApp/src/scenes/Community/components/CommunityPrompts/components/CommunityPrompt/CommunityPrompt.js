import React, { useContext } from "react";
import classes from "./CommunityPrompt.module.css";
import BottomActionBar from "./components/BottomActionBar/BottomActionBar";
import Comments from "./components/Comments/Comments";
import UserAvatar from "../../../../../../components/UserAvatar/UserAvatar";
import { useState } from "react";
import GlobalContext from "../../../../../../context/GlobalContext";

const CommunityPrompt = ({ data, setCommunityPrompts }) => {
    const {
        id,
        question,
        answer,
        likesCount,
        commentsCount,
        user
    } = data;
    
    const { darkMode } = useContext(GlobalContext);
    const [displayComments, setDisplayComments] = useState(false);
    
    return (
        <div className={classes.communityPrompt + (darkMode ? " " + classes.dark : "")}>
            <div className={classes.user}>
                <UserAvatar
                    user={user}
                />
                <div>
                    <div className={classes.title}>{user.firstName + " " + user.lastName}</div>
                    <div>{user.jobTitle}</div>
                </div>
            </div>
            <div className={classes.questionContainer}>
                <h6>Question</h6>
                {
                    question
                        ?.split("\n")
                        ?.map((paragraph, index) => (
                            <p key={index}>
                                {paragraph}
                            </p>
                        ))
                }
            </div>
            <div className={classes.answerContainer}>
                <h6>Answer</h6>
                {
                    answer
                        ?.split("\n")
                        ?.map((paragraph, index) => (
                            <p key={index}>
                                {paragraph}
                            </p>
                        ))}
            </div>
            <BottomActionBar
                communityPrompt={data}
                setCommunityPrompts={setCommunityPrompts}
                setDisplayComments={setDisplayComments}
            />
            <Comments
                communityPromptId={id}
                setCommunityPrompts={setCommunityPrompts}
                displayComments={displayComments}
                setDisplayComments={setDisplayComments}
            />
        </div>
    )
};

export default CommunityPrompt;