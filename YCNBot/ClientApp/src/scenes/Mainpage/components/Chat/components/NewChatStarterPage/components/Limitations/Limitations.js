import React from "react";
import SectionHeading from "../SectionHeading/SectionHeading";
import ReportProblemIcon from '@mui/icons-material/ReportProblem';
import InformationCard from "../InformationCard/InformationCard";

const Limitations = () => {
    return (
        <>
            <SectionHeading
                heading="Warnings"
                icon={<ReportProblemIcon/>}
            />
            {
                [
                    "May occasionally produce inaccurate responses",
                    "Can occasionally produce biased content",
                    "Limited understanding of world and events after 2021"
                ]
                .map(limitation => (
                    <InformationCard
                        key={limitation}
                    >
                        {limitation}
                    </InformationCard>
                ))
            }
        </>
    )
};

export default Limitations;