import React, { useContext } from "react";
import classes from "./BottomSection.module.css";
import DarkMode from "./components/DarkMode/DarkMode";
import Logout from "./components/Logout/Logout";
import ShowUserReport from "./components/ShowUserReport/ShowUserReport";
import GlobalContext from "../../../../../../context/GlobalContext";
import UserFeedback from "./components/UserFeedback/UserFeedback";

const BottomSection = ({ setShowUserReport }) => {
    const { user } = useContext(GlobalContext);
    return (
        <div className={classes.bottomSection}>
            <DarkMode/>
            <UserFeedback/>
            {
                user?.isAdmin &&
                    <ShowUserReport
                        setShowUserReport={setShowUserReport}
                    />
            }
            <Logout/>
        </div>
    )
};

export default BottomSection;