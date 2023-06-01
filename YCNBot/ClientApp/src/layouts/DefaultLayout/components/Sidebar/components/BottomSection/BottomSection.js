import React, { useContext } from "react";
import classes from "./BottomSection.module.css";
import DarkMode from "./components/DarkMode/DarkMode";
import Logout from "./components/Logout/Logout";
import UserReport from "./components/ShowUserReport/ShowUserReport";
import GlobalContext from "../../../../../../context/GlobalContext";
import UserFeedback from "./components/UserFeedback/UserFeedback";
import Usage from "./components/Usage/Usage";
import Community from "./components/Community/Community";

const BottomSection = () => {
    const { user } = useContext(GlobalContext);

    return (
        <div className={classes.bottomSection}>
            <Community/>
            <DarkMode/>
            <UserFeedback/>
            {
                user?.isAdmin &&
                    <>
                        <UserReport/>                    
                        <Usage/>
                    </>
            }
            <Logout/>
        </div>
    )
};

export default BottomSection;