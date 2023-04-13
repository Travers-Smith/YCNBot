import React, { useContext } from "react";
import GlobalContext from "../../../../../../../../context/GlobalContext";
import classes from "./InformationCard.module.css";

const InformationCard = ({ children, onClick }) => {
    const { darkMode } = useContext(GlobalContext);
    
    return (
        <div 
            className={`${classes.informationCard} ${darkMode ? classes.informationCardDark : classes.informationCardLight} ${onClick ? classes.clickable : ""}`}
            onClick={onClick}
        >
            {children}
        </div>
    )
};

export default InformationCard;