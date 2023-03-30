// FetchDataComponent.tsx
import React, { useContext, useEffect } from "react";
import Api from "../api/api";
import { AppContext } from "../AppContext";

const FetchDataComponent: React.FC = () => {
    const { state, updateState } = useContext(AppContext);

    useEffect(() => {
        async function fetchData() {
            updateState({ error: null });
            try {
                const result = await Api.chatAll();
                var conv = result[0];
                updateState({
                    conversation: conv,
                    conversations: result,
                    points: conv.posts![0].points
                });
            } catch (error) {
                updateState({ error: error as any });
            }
        }

        fetchData();
    }, []);

    return (
        <>
        </>
    );
};

export default FetchDataComponent;
