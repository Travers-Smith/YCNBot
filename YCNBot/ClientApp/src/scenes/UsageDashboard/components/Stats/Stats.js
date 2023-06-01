import React from "react";
import classes from "./Stats.module.css";
import StatPanel from "./components/StatPanel/StatPanel";

const Stats = () => {
    return (
        <div className={classes.stats}>
            <StatPanel
                label="Total Chats"
                url="/api/chat/get-chat-count"
            />
            <StatPanel
                label="Total Messages"
                url="/api/message/get-count"
            />
            <StatPanel
                label="Total Users"
                url="/api/chat/get-user-count"
            />
        </div>
    )
};

export default Stats;