import React, { useContext } from "react";
import classes from "./Comment.module.css";
import UserAvatar from "../../../../../../../../../../components/UserAvatar/UserAvatar";
import GlobalContext from "../../../../../../../../../../context/GlobalContext";

const Comment = ({ data }) => {
    const {
        comment,
        dateAdded: dateAddedString,
        user
    } = data;
    
    const { darkMode } = useContext(GlobalContext);
    
    let timeSinceComment = "";

    let date1 = new Date();

    let date2 = new Date(dateAddedString);

    let diff = date1.getTime() - date2.getTime();

    let seconds = Math.floor(diff / 1000);

    let minutes = Math.floor(seconds / 60);

    let hours = Math.floor(minutes / 60);

    let days = Math.floor(hours / 24);

    let months = (date2.getFullYear() - date1.getFullYear()) * 12 + (date2.getMonth() - date1.getMonth());

    if(seconds < 60){
        timeSinceComment = seconds + " seconds"
    } else if(minutes < 60){
        timeSinceComment = minutes + " minutes"
    } else if(hours < 24){
        timeSinceComment = hours + " hours"
    } else if(days < 31){
        timeSinceComment = days + " days"
    } else {
        timeSinceComment = months + " months"
    }

    return (
        <div className={classes.comment}>
            <UserAvatar
                user={user}
            />
            <div className={classes.commentText}>
                <div className={classes.topRow}>
                    <div className={classes.header}>
                        <div className={classes.name}>{user.firstName} {user.lastName}</div>
                        <div className={classes.time}>
                            {timeSinceComment} ago
                        </div>
                    </div>
                    <div className={classes.jobTitle}>{user.jobTitle}</div>
                </div>
                <div>
                    {comment}
                </div>
            </div>
        </div>
    )
};

export default Comment;