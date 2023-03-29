import React, { useContext, useState } from "react";
import { Form, InputGroup, Input, Button, Spinner } from "reactstrap";
import Api from "../api/api";
import { AppContext } from "../appContext";

const SearchForm: React.FC = () => {
    const { state, setState } = useContext(AppContext);
    const [searchQuery, setSearchQuery] = useState("");

    const handleSearchChange = (event: any) => {
        setSearchQuery(event.target.value);
    };

    const handleSearchSubmit = async (event: any) => {
        event.preventDefault();

        try {
            const result = await Api.chat(searchQuery);
            setState({
                ...state,
                searchResult: result,
              });
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <Form onSubmit={handleSearchSubmit} className="d-flex flex-fill me-2">
            <InputGroup>
                <Input
                    type="text"
                    placeholder="Search..."
                    value={searchQuery}
                    onChange={handleSearchChange}
                />
                <Button type="submit" color="primary">
                    {state.searchResult.items.length === 0 ? (
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
