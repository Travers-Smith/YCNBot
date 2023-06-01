import React from "react";
import BottomSection from "./components/BottomSection/BottomSection";
import ChatHistory from "./components/ChatHistory/ChatHistory";
import NewChat from "./components/NewChat/NewChat";
import classes from "./Sidebar.module.css";

const Sidebar = ({ previousChats, setPreviousChats, setShowUserReport, chat }) => {
    return (
        <div className={classes.sidebar}>
            <NewChat/>
            <ChatHistory
                chat={chat}
                previousChats={previousChats}
                setPreviousChats={setPreviousChats}
            />
            <BottomSection
                setShowUserReport={setShowUserReport}
            />
        </div>
    );
};

export default Sidebar;