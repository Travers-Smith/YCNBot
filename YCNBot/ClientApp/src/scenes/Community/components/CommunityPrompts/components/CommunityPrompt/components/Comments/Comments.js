import React, { useEffect, useState } from "react";
import classes from "./Comments.module.css";
import useSafeDataFetch from "../../../../../../../../hooks/useSafeDataFetch";
import Comment from "./components/Comment/Comment";
import NewComment from "./components/NewComment/NewComment";
import ShowMore from "../../../../../../../../layouts/DefaultLayout/components/Sidebar/components/ChatHistory/components/ShowMore/ShowMore";

const Comments = ({ communityPromptId, displayComments, setDisplayComments }) => {
    const [comments, setComments] = useState([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [hasMore, setHasMore] = useState(true);
    const [{ isLoading }, safeDataFetch] = useSafeDataFetch();

    useEffect(() => {
        const fetchData = async () => {
            const response = await safeDataFetch({
                url: `/api/community-prompt-comment/get-by-community-prompt-id?communityPromptId=${communityPromptId}&pageNumber=${pageNumber}`
            });

            if(!response.isError){
                setHasMore(response.data.length > 0);

                setComments(comments => [
                    ...comments,
                    ...response.data
                ]);
            }
        };

        if(displayComments){
            fetchData();
        }
    }, [displayComments, pageNumber]);

    return (
        <div>
            {
                (comments.length > 0 && displayComments) &&
                    <>
                        <div className={classes.comments}>
                            {
                                comments.map((comment, index) => (
                                    <Comment
                                        key={index}
                                        data={comment}
                                    />
                                ))
                            }
                            <ShowMore
                                setPageNumber={setPageNumber}
                                hasMore={hasMore}
                            />
                        </div>
                    </>
            }
            <NewComment
                communityPromptId={communityPromptId}
                setComments={setComments}
                displayComments={displayComments}
                setDisplayComments={setDisplayComments}
            />
        </div>
    )
};

export default Comments;