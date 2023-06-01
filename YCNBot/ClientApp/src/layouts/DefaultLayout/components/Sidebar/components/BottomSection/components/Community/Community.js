import React from "react";
import classes from "./Community.module.css";
import PeopleIcon from '@mui/icons-material/People';
import { Link } from "react-router-dom";

const Community = () => {
    return (
        <Link 
            className={classes.community}
            to="/community" 
            >
            <PeopleIcon/>           
            <span className={classes.text}>
                Community
            </span>
        </Link>
    )
};

export default Community;