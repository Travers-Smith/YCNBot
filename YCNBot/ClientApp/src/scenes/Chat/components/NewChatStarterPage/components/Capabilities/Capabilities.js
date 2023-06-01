import React from "react";
import SectionHeading from "../SectionHeading/SectionHeading";
import ElectricBoltIcon from '@mui/icons-material/ElectricBolt';
import InformationCard from "../InformationCard/InformationCard";

const Capabilities = () => {
    return (
        <>
            <SectionHeading
                heading="Features"
                icon={<ElectricBoltIcon/>}
            />
            {
                [
                    "Remembers the whole conversation",
                    "User can provide follow-up corrections",
                    "Trained to not accept inappropriate messages"
                ]
                .map(capability => (
                    <InformationCard
                        key={capability}
                    >
                        {capability}
                    </InformationCard>
                ))
            }
        </>
    )
};

export default Capabilities;