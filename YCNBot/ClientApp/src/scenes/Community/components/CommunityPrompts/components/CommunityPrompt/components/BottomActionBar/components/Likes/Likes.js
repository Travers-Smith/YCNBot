import React,  { useEffect, useState }  from "react";
import classes from "./Likes.module.css";
import { 
    Button, 
    Dialog, 
    DialogActions, 
    DialogContent, 
    DialogContentText, 
    DialogTitle } from "@mui/material";
import useSafeDataFetch from "../../../../../../../../../../hooks/useSafeDataFetch";
import UserLike from "./components/UserLike/UserLike";
import PaginationButton from "../../../../../../../../../../components/PaginationButton/PaginationButton";

const Likes = ({ likesCount, communityPromptId, userLikes, setUserLikes }) => {
    const [open, setOpen] = useState(false);
    const [{ isLoading, isError}, safeDataFetch] = useSafeDataFetch();
    const [pageNumber, setPageNumber] = useState(1);
    const [hasMore, setHasMore] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            const response = await safeDataFetch({
                url: `/api/community-prompt-like/get-by-community-prompt-id?communityPromptId=${communityPromptId}&pageNumber=${pageNumber}`
            });
            if(!response.isError){
                setHasMore(response.data.length > 0);

                if(pageNumber === 1){
                    setUserLikes(response.data);
                } else {
                    setUserLikes(userLikes => [...userLikes, ...response.data]);
                }
            }
        };

        if(open){
            fetchData();
        }
    }, [open, pageNumber]);

    return (
        <>
           <Dialog 
                open={open} 
                onClose={() => setOpen(false)}
            >
                <DialogTitle>Likes</DialogTitle>
                <DialogContent>
                    <div className={classes.userLikes}>
                        {
                            userLikes.map(ul => (
                                <UserLike
                                    key={ul.email}  
                                    user={ul}
                                    />
                                    ))
                                
                        }
                        <PaginationButton
                            hasMore={hasMore}
                            isLoading={isLoading}
                            setPageNumber={setPageNumber}
                        />
                        {
                            isError &&
                            <>
                                    <br/>
                                    <DialogContentText
                                        color="red"
                                        >
                                        Unable to get likes.
                                    </DialogContentText>
                                </>
                        }
                    </div>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpen(false)}>
                        Close
                    </Button>
                </DialogActions>
            </Dialog>
        {
            likesCount > 0 &&
                <Button
                    onClick={() => setOpen(true)}
                >
                    {likesCount} Likes
                </Button>
        }
        </>
    );
}

export default Likes;