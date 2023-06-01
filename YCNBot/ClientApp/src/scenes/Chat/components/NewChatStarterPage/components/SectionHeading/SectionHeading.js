import React, { useContext } from "react";
import GlobalContext from "../../../../../../context/GlobalContext";
import classes from "./SectionHeading.module.css";

const SectionHeading  = ({ icon, heading }) => {
    const { darkMode } = useContext(GlobalContext);

    return (
        <div className={`${classes.sectionHeading} ${darkMode ? classes.sectionHeadingDark : classes.sectionHeadingLight}`}>
            <div>
                {icon}
            </div>
            <div>
                {heading}
            </div>
        </div>
    )
};

export default SectionHeading;