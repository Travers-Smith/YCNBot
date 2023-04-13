import { createContext } from "react";

const GlobalContext = createContext({
    darkMode: null,
    setDarkMode: null
});

export default GlobalContext;