import React, { useContext, useState } from "react";
import { Form, InputGroup, Input, Button } from "reactstrap";
import Api from "../api/api";
import { AppContext } from "../AppContext";

const UploadForm: React.FC = () => {
    const { state, updateState } = useContext(AppContext);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        setSelectedFile(file || null);
    };

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (selectedFile) {
            try {

                const blob = new Blob([selectedFile], { type: selectedFile.type });

                const response = await Api.upload(blob);

                // updateState({
                //     ...state,
                //     locations: [...state.locations, response.location],
                //   });


                console.log(response);
            } catch (error) {
                console.error(error);
            }
        }
    };

    return (
        <Form onSubmit={handleSubmit} className="d-flex">
            <InputGroup>
                <Input type="file" onChange={handleFileChange} placeholder="file..." />
                <Button type="submit" color="primary" disabled={!selectedFile}>
                    Upload
                </Button>
            </InputGroup>
        </Form>
    );
};

export default UploadForm;
