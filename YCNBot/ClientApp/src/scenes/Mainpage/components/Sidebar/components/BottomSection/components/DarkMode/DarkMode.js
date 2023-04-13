import React, { useContext } from "react";
import ActionButton from "../ActionButton/ActionButton";
import LightModeIcon from '@mui/icons-material/LightMode';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import GlobalContext from "../../../../../../../../context/GlobalContext";

const DarkMode = () => {
    const {
        darkMode,
        setDarkMode
    } = useContext(GlobalContext);

    const changeDarkMode = () => {
        sessionStorage.setItem("darkMode", !darkMode)
        setDarkMode(!darkMode);
    }
    return (
        <ActionButton
            onClick={changeDarkMode}
            icon={darkMode ? <LightModeIcon/> : <DarkModeIcon/>}
        >
            {
                darkMode ?
                    "Light Mode"
                : 
                    "Dark Mode"
            }
        </ActionButton>
    )
};

export default DarkMode;