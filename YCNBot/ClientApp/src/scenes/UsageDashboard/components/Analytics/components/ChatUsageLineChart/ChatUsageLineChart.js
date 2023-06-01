import React, { useEffect, useState } from "react";
import classes from "./ChatUsageLineChart.module.css";
import {
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    BarChart,
    Bar,
    Legend
  } from "recharts";
import useSafeDataFetch from "../../../../../../hooks/useSafeDataFetch";
import { useRef } from "react";

const ChatUsageLineChart = () => {
    const safeDataFetch = useSafeDataFetch()[1];
    const [history, setHistory] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            const response = await safeDataFetch({
                url: "/api/message/get-message-breakdown"
            });

            if(!response.isError){
                setHistory(response.data);
            }
        };

        fetchData();
    }, []); 

    const parentRef = useRef();

    return (
        <div className={classes.chatUsageLineChart} ref={parentRef}>
            <h5>Activity</h5>
            <BarChart 
                width={parentRef?.current?.offsetWidth - 15}
                height={400}
                data={history}
            >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="dateAdded" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Bar dataKey="messages" fill="#82ca9d" />
            </BarChart>
        </div>
    );
};

export default ChatUsageLineChart;