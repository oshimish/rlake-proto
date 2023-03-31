import React, { useContext, useState } from "react";
import { Form, InputGroup, Input, Button, Spinner, Modal, ModalBody } from "reactstrap";
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
            updateState({ error: null });
            setLoading(true);
            const result = await Api.start(searchQuery);
            state.conversations.push(result.conversation)
            updateState({
                conversation: result.conversation,
                conversations: state.conversations,
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
                    placeholder="Ask ChatGPT about any places you want"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    autoComplete="on"
                />
                <Button type="submit" color="primary">
                    {!loading ? (
                        "Ask"
                    ) : (
                        <Spinner size="sm" color="light" />
                    )}
                </Button>
            </InputGroup>


            <Modal isOpen={loading} centered>
                <ModalBody className="d-flex m-4">
                    <div className="row  justify-content-center align-items-center">
                        <div className="col-2" >
                            {<Spinner size="lg" color="primary" className="spinner-border  text-success" >
                            </Spinner>}</div>
                        <div className="col-10">
                            <span>Be patient, I'm a bot not a magician! Loading... <br />
                                It can take a while (I'm really busy)</span></div>
                    </div>

                </ModalBody>
            </Modal>
        </Form>
    );
};

export default SearchForm;
