import React, { useContext } from "react";
import classes from "./Message.module.css";
import GlobalContext from "../../../../../../context/GlobalContext";
import { Avatar } from "@mui/material";
import MessageRating from "./components/MessageRating/MessageRating";
import ReportProblemIcon from '@mui/icons-material/ReportProblem';
import AddToCommunity from "./components/AddToCommunity/AddToCommunity";
import UserAvatar from "../../../../../../components/UserAvatar/UserAvatar";

const Message = ({ data, recordChat, questionText }) => {
    const {
        isSystem,
        pending,
        rating,
        text,
        uniqueIdentifier,
        isError,
        isPiError,
        containsPI
    } = data;

    const {
        darkMode,
        user
    } = useContext(GlobalContext);    

    let className;

    if(isSystem && darkMode){
        className = classes.systemDark;
    } else if (isSystem && !darkMode){
        className = classes.systemLight;
    } else if (!isSystem && darkMode){
        className = classes.userDark;
    } else {
        className = classes.userLight;
    }

    return (
        <div 
            className={`${classes.message} ${className}`}
        >
            {
                isSystem ?
                    <Avatar/>
                :
                    <UserAvatar
                        user={user}
                    />
            }
            <div className={classes.messageBody}>
                {
                    pending ?
                        <div className={`${classes.loading} ${darkMode ? classes.loadingDark : classes.loadingWhite}`}/>
                    :
                        <>
                            <div>
                                {
                                    (!isError && text?.length > 0) ?
                                        text?.split("\n")?.map((paragraph, index) => (
                                            <p key={index}>
                                                {paragraph}
                                            </p>
                                        ))
                                    : isError &&
                                        <div className={classes.error}>
                                            <ReportProblemIcon
                                                color="danger"
                                            />
                                            <div>
                                                {
                                                    isPiError ?
                                                        "There was personal information in this message"
                                                    :
                                                        "TSBot is undergoing maintenance - please check back later!"
                                                }
                                            </div>
                                        </div>
                                }
                                {
                                    containsPI &&
                                        <div className={classes.error}>
                                            <ReportProblemIcon
                                                color="danger"
                                            />
                                            <div>
                                                There was personal information in this message                                            
                                            </div>
                                        </div>
                                }
                            </div>
                            {
                                (isSystem && !isError && recordChat) &&
                                    <MessageRating
                                        currentRating={rating}
                                        messageIdentifier={uniqueIdentifier}
                                    />
                            }
                            {
                                (isSystem && !isError) &&
                                    <AddToCommunity
                                        questionText={questionText}
                                        answerText={text}
                                    />
                            }
                        </>
                }
            </div>
        </div>
    )
};

export default Message;