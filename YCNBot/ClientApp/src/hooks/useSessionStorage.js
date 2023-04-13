import { useEffect, useState } from "react";

const useSessionStorage = sessionStorageKey =>  {
    const [value, setValue] = useState(sessionStorage.getItem(sessionStorageKey) || "");

    useEffect(() => {
        sessionStorage.setItem(sessionStorageKey, value);
    }, [value, setValue, sessionStorageKey]);

    return [value, setValue]
};

export default useSessionStorage;