import React from "react";
import classes from "./Login.module.css";
import { Button } from "@mui/material";
import config from "../../config.json";

const Login = () => {
    return (
        <div className={classes.login}>
            <div>
                Welcome to {config.APP_NAME}
            </div>
            <div>
                Login with your company account to continue
            </div>
            <Button
                onClick={() => window.location.href = "/api/auth/login"}
            >
                Login
            </Button>
        </div>
    );
};

export default Login;