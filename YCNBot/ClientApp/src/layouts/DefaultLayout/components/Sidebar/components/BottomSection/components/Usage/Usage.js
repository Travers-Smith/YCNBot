import React from "react";
import classes from "./Usage.module.css";
import { Link } from "react-router-dom";
import BarChartIcon from '@mui/icons-material/BarChart';

const Usage = () => {
    return (
        <Link 
            className={classes.usage}
            to="/usage" 
            >
            <BarChartIcon/>           
            <span className={classes.text}>
                Usage
            </span>
        </Link>
    )
};

export default Usage;