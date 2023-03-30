import React, { useContext, useState } from "react";
import { Form, InputGroup, Input, Button, Spinner } from "reactstrap";
import Api from "../api/api";
import { AppContext } from "../AppContext";

const SearchForm: React.FC = () => {
    const { state, updateState } = useContext(AppContext);
    const [searchQuery, setSearchQuery] = useState("");
    const [loading, setLoading] = useState(false);

    const handleSearchChange = (event: any) => {
        setSearchQuery(event.target.value);
    };

    const handleSearchSubmit = async (event: any) => {
        event.preventDefault();

        try {
            setLoading(true);
            const result = await Api.start(searchQuery);
            updateState({
                points: result.items,
            });
        } catch (error) {
            updateState({ error: error as any });
        }
        setLoading(false);
    };

    return (
        <Form onSubmit={handleSearchSubmit} className="d-flex flex-fill mx-4">
            <InputGroup>
                <Input
                    type="text"
                    placeholder="Search..."
                    value={searchQuery}
                    onChange={handleSearchChange}
                />
                <Button type="submit" color="primary">
                    {!loading ? (
                        "Search"
                    ) : (
                        <Spinner size="sm" color="light" />
                    )}
                </Button>
            </InputGroup>
        </Form>
    );
};

export default SearchForm;
