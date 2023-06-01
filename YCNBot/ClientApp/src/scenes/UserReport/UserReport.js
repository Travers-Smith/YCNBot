import React, { useContext, useEffect, useState } from "react";
import classes from "./UserReport.module.css";
import useSafeDataFetch from "../../hooks/useSafeDataFetch";
import { Button, CircularProgress } from "@mui/material";
import GlobalContext from "../../context/GlobalContext";

const UserReport = () => {
    const [users, setUsers] = useState([]);
    
    const [{ isLoading, isError }, safeDataFetch] = useSafeDataFetch({});
    
    const {
        darkMode
    } = useContext(GlobalContext);
    
    const [pageNumber, setPageNumber] = useState(1);

    useEffect(() => {
        const fetchUsers = async () => {
            const response = await safeDataFetch({
                url: "/api/user-reporting/get-active-users"
            });

            if(!response.isError){
                if(pageNumber === 1){
                    setUsers(response.data);
                } else {
                    setUsers(users => [...users, ...response.data]);
                }
            }
        }

        fetchUsers();
    }, [setPageNumber]);

    return (
        <div className={classes.userReportContainer + ` ${darkMode ? classes.dark : classes.light}`}>
            <h4>User Activity</h4>
            <hr/>
            <div className={classes.userReport}>
                {
                    isLoading ?
                        <div className={classes.loadingContainer}>
                            <CircularProgress/>
                        </div>

                    : isError ?
                        <div>Warning</div>
                    :
                    <table>
                        <thead>
                            <tr>
                                <th>
                                    Name
                                </th>
                                <th>
                                    Email
                                </th>
                                <th>
                                    Department
                                </th>
                                <th>
                                    Job Title
                                </th>
                                <th>
                                    Chats
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                users.map(userUsage => {
                                    const {
                                        firstName,
                                        lastName, 
                                        email,
                                        department,
                                        jobTitle
                                    } = userUsage.user;

                                    return (
                                        <tr>
                                            <td>
                                                {`${firstName} ${lastName}`}
                                            </td>
                                            <td>
                                                {email}
                                            </td>
                                            <td>
                                                {department}
                                            </td>
                                            <td>
                                                {jobTitle}
                                            </td>
                                            <td>{userUsage.totalChats}</td>
                                        </tr>
                                )})
                            }
                        </tbody>
                    </table>
                }
                <div className={classes.showMoreContainer}>
                    <Button className={classes.showMore}>
                        Show More
                    </Button>
                </div>
            </div>
        </div>
    )
};

export default UserReport;