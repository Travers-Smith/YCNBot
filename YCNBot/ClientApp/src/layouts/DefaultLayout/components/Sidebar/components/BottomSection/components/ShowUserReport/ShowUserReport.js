import React from "react";
import classes from "./UserReport.module.css";
import SummarizeIcon from '@mui/icons-material/Summarize';
import { Link } from "react-router-dom";

const UserReport = () => {
    return (
        <Link 
            className={classes.userReport}
            to="/user-usage" 
            >
            <SummarizeIcon/>           
            <span className={classes.text}>
                User Reports
            </span>
        </Link>
    )
};

export default UserReport;