import React from "react";
import classes from "./Logout.module.css";
import LogoutIcon from '@mui/icons-material/Logout';

const Logout = () => {
    return (
        <a href="/api/auth/logout" className={classes.logout}>
            <LogoutIcon
                fontSize="small"
            />
            <div className={classes.text}>
                Logout
            </div>
        </a>
    )
};

export default Logout;