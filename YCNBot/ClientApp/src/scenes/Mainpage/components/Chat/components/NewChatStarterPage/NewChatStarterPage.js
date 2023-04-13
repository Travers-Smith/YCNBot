import React, { useContext } from "react";
import classes from "./NewChatStarterPage.module.css";
import Examples from "./components/Examples/Examples";
import Capabilities from "./components/Capabilities/Capabilities";
import Limitations from "./components/Limitations/Limitations";
import GlobalContext from "../../../../../../context/GlobalContext";
import config from "../../../../../../config.json";
const NewChatStarterPage = ({ newMessageRef }) => {
    const { darkMode } = useContext(GlobalContext);

    return (
        <div className={classes.exampleMessages}>
            <div className={classes.headingContainer}>
                <h2 className={darkMode ? classes.darkHeading : classes.lightHeading}>
                    {config.APP_NAME}
                </h2>
            </div>
            <div className={classes.body}>
                <Examples
                    newMessageRef={newMessageRef}
                />
                <Capabilities/>
                <Limitations/>
            </div>
        </div>
    )
};

export default NewChatStarterPage;

