import React, { useEffect, useState } from "react";
import classes from "./StatPanel.module.css";
import useSafeDataFetch from "../../../../../../hooks/useSafeDataFetch";

const StatPanel = ({ label, url }) => {
    const [{ isLoading }, safeDataFetch] = useSafeDataFetch();

    const [value, setValue] = useState(null);
    useEffect(() => {
        const fetchData = async () => {
            const response = await safeDataFetch({
                url: url
            });

            if(response.isError){
                setValue("N/A");
            } else {
                setValue(response.data);
            }
        }

        fetchData();
    }, []);

    return (
        <div className={classes.statPanel}>
            <h6>{label}</h6>
            <h2>{value}</h2>
        </div>
    );
};

export default StatPanel;