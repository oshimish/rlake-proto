// FetchDataComponent.tsx
import React, { useContext, useEffect } from "react";
import { Modal, ModalBody, Spinner } from "reactstrap";
import Api from "../api/api";
import { AppContext } from "../AppContext";

const FetchDataComponent: React.FC = () => {
    const { state, updateState } = useContext(AppContext);

    useEffect(() => {
        async function fetchData() {
            updateState({ error: null, loading: true });
            try {
                const result = await Api.listConverstations();
                var conv = result[0];
                updateState({
                    conversation: conv,
                    conversations: result,
                    points: conv.posts![0].points
                });
            } catch (error) {
                updateState({ error: error as any });
            }
            updateState({ loading: false });
        }



        fetchData();
    }, []);

    return (
        <Modal isOpen={state.loading} centered>
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

    );
};

export default FetchDataComponent;
