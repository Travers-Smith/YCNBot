import React from "react";
import classes from "./Community.module.css";
import CommunityPrompts from "./components/CommunityPrompts/CommunityPrompts";
import { useContext } from "react";
import GlobalContext from "../../context/GlobalContext";

const Community = () => {
    const { darkMode } = useContext(GlobalContext);

    return (
        <div className={classes.community + (darkMode ? " " + classes.dark : "")}>
            <h5>Community</h5>
            <hr/>
            <CommunityPrompts/>
        </div>
    )
};

export default Community;