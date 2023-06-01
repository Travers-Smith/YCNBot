import React, { useEffect, useReducer, useState } from "react";
import axios from "axios";
import fetchDataReducer from "../reducers/fetchDataReducer";

const useSafeDataFetch = () => {
    const [data, dispatch] = useReducer(fetchDataReducer, {
        isLoading: false, 
        isError: false,
        errorMessage: null
    });
    
    let signal = null;
    
    const fetchData = async ({ data, method, params, url }) => {
        dispatch({ type: "INIT" });

        try{      
            const { data: responseData } = await axios({
                data: data,
                method: method,
                params: params,
                signal: signal,
                url: url
            });

            dispatch({ type: "SUCCESS" });

            return {
                data: responseData,
                isError: false
            };

        } catch(e) {
            const errorMessage = e?.response?.data;

            dispatch({ type: "FAILURE", payload: errorMessage });
            return {
                isError: true,
                errorMessage: errorMessage
            }
        }
    }

    useEffect(() => {
        let abortController = new AbortController();

        signal = abortController.signal;

        return () => abortController.abort();
    }, []);

        
    return [data, fetchData];
}

export default useSafeDataFetch;