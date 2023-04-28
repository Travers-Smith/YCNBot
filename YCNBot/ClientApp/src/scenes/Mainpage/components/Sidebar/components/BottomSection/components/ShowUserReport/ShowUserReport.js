import React from "react";
import ActionButton from "../ActionButton/ActionButton";
import SummarizeIcon from '@mui/icons-material/Summarize';

const ShowUserReport = ({ setShowUserReport }) => {
    return (
        <ActionButton
            onClick={() => setShowUserReport(true)}
            icon={<SummarizeIcon/>}
        >
            User Report
        </ActionButton>
    )
};

export default ShowUserReport;