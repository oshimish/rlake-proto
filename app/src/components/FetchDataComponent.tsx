// FetchDataComponent.tsx
import React, { useContext, useEffect, useState } from "react";
import Api from "../api/api";
import { AppContext } from "../AppContext";
import ErrorAlert from "./ErrorAlert";

const FetchDataComponent: React.FC = () => {
    const { state, updateState } = useContext(AppContext);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        async function fetchData() {
            setError(null);
            try {
                // Use your API client to fetch data here
                const result = await Api.chatAll();
                var conv = result[0];
                updateState({ points: conv.posts![0].points });
            } catch (error) {
                setError(error as Error);
            }
        }

        fetchData();
    }, []);

    return (
        <>
            <ErrorAlert error={error} />
        </>
    );
};

export default FetchDataComponent;
