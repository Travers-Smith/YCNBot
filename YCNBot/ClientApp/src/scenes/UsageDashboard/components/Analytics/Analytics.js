import React from "react";
import classes from "./Analytics.module.css";
import ChatUsageLineChart from "./components/ChatUsageLineChart/ChatUsageLineChart";

const Analytics = () => {
    return (
        <div className={classes.analytics}>
            <ChatUsageLineChart/>
        </div>
    )
};

export default Analytics;