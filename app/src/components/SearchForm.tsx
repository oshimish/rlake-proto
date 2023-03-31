import React, { useContext, useState } from "react";
import { Form, InputGroup, Input, Button, Spinner, Modal, ModalBody } from "reactstrap";
import Api from "../api/api";
import { AppContext } from "../AppContext";

const SearchForm: React.FC = () => {
    const { state, updateState } = useContext(AppContext);
    const [searchQuery, setSearchQuery] = useState("");

    const handleSearchChange = (event: any) => {
        setSearchQuery(event.target.value);
    };

    const handleSearchSubmit = async (event: any) => {
        event.preventDefault();

        try {
            updateState({ error: null, loading: true });
            const result = await Api.start(searchQuery);
            state.conversations.push(result.conversation)
            updateState({
                conversation: result.conversation,
                conversations: state.conversations,
                points: result.items,
            });
        } catch (error) {
            updateState({ error: error as any, loading: false });
        }
        updateState({ loading: false });
    };

    return (
        <Form onSubmit={handleSearchSubmit} className="d-flex flex-fill mx-4">
            <InputGroup>
                <Input
                    type="text"
                    placeholder="Ask ChatGPT about any places you want"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    autoComplete="on"
                />
                <Button type="submit" color="primary">
                    {!state.loading ? (
                        "Ask"
                    ) : (
                        <Spinner size="sm" color="light" />
                    )}
                </Button>
            </InputGroup>
        </Form>
    );
};

export default SearchForm;
