import React from "react";
import classes from "./DefaultLayout.module.css";
import Sidebar from "./components/Sidebar/Sidebar";
import { Outlet } from "react-router-dom";

const DefaultLayout = ({ chat, previousChats, setPreviousChats }) => {
    return (
        <div className={classes.defaultLayout}> 
            <Sidebar
                chat={chat}
                previousChats={previousChats}
                setPreviousChats={setPreviousChats}
            />
            <Outlet/>
        </div>
    )
};

export default DefaultLayout;