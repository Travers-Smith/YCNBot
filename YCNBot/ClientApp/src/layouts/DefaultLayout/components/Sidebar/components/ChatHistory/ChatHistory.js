import React, { useEffect, useState } from "react";
import classes from "./ChatHistory.module.css";
import useSafeDataFetch from "../../../../../../hooks/useSafeDataFetch";
import PreviousChat from "./components/PreviousChat/PreviousChat";
import ReportProblemIcon from '@mui/icons-material/ReportProblem';
import ShowMore from "./components/ShowMore/ShowMore";

const ChatHistory = ({ chat, previousChats, setPreviousChats }) => {
    const [hasMore, setHasMore] = useState(true);
    const [pageNumber, setPageNumber] = useState(1);
    const [{ isError }, safeDataFetch] = useSafeDataFetch();

    useEffect(() => {
        const fetchChatHistory = async () => {
            const response = await safeDataFetch({
                params: {
                    pageNumber: pageNumber
                },
                url: "/api/chat/get-all-by-user"
            });

            if(!response.isError){
                setHasMore(response.data.length > 0);
                if(pageNumber === 1){
                    setPreviousChats(response.data)
                } else {
                    setPreviousChats(previousChats => [...previousChats, ...response.data])
                }
            }
        }

        fetchChatHistory();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pageNumber, setPreviousChats])

    return (
        <div>
            <div className={classes.chatHistory}>
                {
                    isError ?
                        <div className={classes.isErrorContainer}>
                            <div className={classes.isError}>
                                <ReportProblemIcon/>
                                <p>Sorry we are unable to retrieve your chat history at the moment</p>
                            </div>
                        </div>
                    :
                        previousChats?.map(previousChat => (
                            <PreviousChat
                                chat={chat}
                                data={previousChat}
                                key={previousChat.uniqueIdentifier}
                                setPreviousChats={setPreviousChats}
                            />
                    ))
                }
            </div>
            <ShowMore
                setPageNumber={setPageNumber}
                hasMore={hasMore}
                dark
            />
        </div>
    )
};

export default ChatHistory;