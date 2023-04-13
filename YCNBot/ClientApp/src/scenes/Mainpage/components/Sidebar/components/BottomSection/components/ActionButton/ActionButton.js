import React from "react";
import classes from "./ActionButton.module.css";

const ActionButton = ({ children, icon, onClick }) => {

    return (
        <div className={classes.logout} onClick={onClick}>
            {icon}
            <div className={classes.text}>
                {children}
            </div>
        </div>
    )
};

export default ActionButton;