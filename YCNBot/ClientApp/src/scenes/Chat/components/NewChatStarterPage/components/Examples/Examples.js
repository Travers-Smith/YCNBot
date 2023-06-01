import React from "react";
import SectionHeading from "../SectionHeading/SectionHeading";
import LightModeIcon from '@mui/icons-material/LightMode';
import InformationCard from "../InformationCard/InformationCard";

const Examples = ({ newMessageRef }) => {
    return (
        <>
            <SectionHeading
                heading="Examples"
                icon={<LightModeIcon/>}
            />
            {
                [
                    "\"Explain the SFDR regulations in simple terms\"",
                    "\"Provide 10 boiler plate warranties for an SPA\"",
                    "\"How do I file a trademark dispute with the USPTO\""
                ].map(example => (
                    <InformationCard
                        key={example}
                        onClick={() => newMessageRef.current.innerText = example.replaceAll("\"", "")}
                    >
                        {example}
                    </InformationCard>
                ))
            }
        </>
    );
}

export default Examples;