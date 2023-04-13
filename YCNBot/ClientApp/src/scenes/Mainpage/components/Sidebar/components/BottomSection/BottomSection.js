import React from "react";
import classes from "./BottomSection.module.css";
import DarkMode from "./components/DarkMode/DarkMode";
import Logout from "./components/Logout/Logout";

const BottomSection = () => {
    return (
        <div className={classes.bottomSection}>
            <DarkMode/>
            <Logout/>
        </div>
    )
};

export default BottomSection;