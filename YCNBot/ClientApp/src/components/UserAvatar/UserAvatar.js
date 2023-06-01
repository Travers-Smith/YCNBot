import { Avatar } from "@mui/material";
import React from "react";

const UserAvatar = ({ user }) => {
    return (
        <Avatar>
            {user?.firstName?.charAt(0)}{user?.lastName?.charAt(0)}
        </Avatar>
    )
};

export default UserAvatar;