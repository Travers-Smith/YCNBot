import React, { useEffect, useState } from "react";
import classes from "./CommunityPrompts.module.css";
import useSafeDataFetch from "../../../../hooks/useSafeDataFetch";
import CommunityPrompt from "./components/CommunityPrompt/CommunityPrompt";
import PaginationButton from "../../../../components/PaginationButton/PaginationButton";

const CommunityPrompts = () => {
    const [communityPrompts, setCommunityPrompts] = useState([]);

    const [{ isLoading }, safeDataFetch] = useSafeDataFetch();
    
    const [pageNumber, setPageNumber] = useState(1);
    const [hasMore, setHasMore] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            const response = await safeDataFetch({
                url: "/api/community-prompt/get?pageNumber=" + pageNumber 
            });

            if(!response.isError){
                setHasMore(response.data.length > 0);

                if(pageNumber === 1){
                    setCommunityPrompts(response.data);
                } else {
                    setCommunityPrompts(cps => [...cps, ...response.data])
                }
            }
        }

        fetchData();
    }, [pageNumber]);

    return (
        <div>
            <div className={classes.promptsContainer}>
                {
                    communityPrompts.map(cp => (
                        <CommunityPrompt
                            key={cp.id}
                            id={cp.id}
                            data={cp}
                            setCommunityPrompts={setCommunityPrompts}
                        />
                    ))
                }
            </div>
            <PaginationButton
                hasMore={hasMore}
                isLoading={isLoading}
                setPageNumber={setPageNumber}
            />
        </div>
    )
};

export default CommunityPrompts;