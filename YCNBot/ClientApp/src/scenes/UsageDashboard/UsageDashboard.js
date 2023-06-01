import React from "react";
import classes from "./UsageDashboard.module.css";
import Stats from "./components/Stats/Stats";
import Analytics from "./components/Analytics/Analytics";

const UsageDashboard = () => {
    return (
        <div className={classes.usageDashboard}>
            <div className={classes.heading}>
                <h4>Usage</h4>
            </div>
            <Stats/>
            <Analytics/>
        </div>
    )
};

export default UsageDashboard;