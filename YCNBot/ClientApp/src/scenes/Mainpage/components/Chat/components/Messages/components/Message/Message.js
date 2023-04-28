import React, { useContext } from "react";
import classes from "./Message.module.css";
import GlobalContext from "../../../../../../../../context/GlobalContext";
import { Avatar } from "@mui/material";
import MessageRating from "./components/MessageRating/MessageRating";
import ReportProblemIcon from '@mui/icons-material/ReportProblem';

const Message = ({ data, recordChat }) => {
    const {
        isSystem,
        pending,
        rating,
        text,
        uniqueIdentifier,
        isError,
        isPiError
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
                    <Avatar>
                        {user?.firstName?.charAt(0)}{user?.lastName?.charAt(0)}
                    </Avatar>
            }
            <div className={classes.messageBody}>
                {
                    pending ?
                        <div className={`${classes.loading} ${darkMode ? classes.loadingDark : classes.loadingWhite}`}>

                        </div>
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
                                                        "I'm unable to talk right now, please try again later!"
                                                }
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
                        </>
                }
            </div>
        </div>
    )
};

export default Message;