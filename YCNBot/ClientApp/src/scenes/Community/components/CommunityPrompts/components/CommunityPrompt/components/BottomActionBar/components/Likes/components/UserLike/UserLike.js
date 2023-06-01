import React from "react";
import classes from "./UserLike.module.css";
import UserAvatar from "../../../../../../../../../../../../components/UserAvatar/UserAvatar";

const UserLike = ({ user }) => {
    const {
        firstName,
        lastName,
        jobTitle
    } = user;

    return (
        <div className={classes.userLike}>
            <UserAvatar
                user={user}
            />
            <div>
                <div className={classes.name}>
                    {firstName} {lastName}
                </div>
                <div>{jobTitle}</div>
            </div>
        </div>
    )
};

export default UserLike;