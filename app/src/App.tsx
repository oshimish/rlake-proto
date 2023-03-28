import React, { useState } from 'react';
import { Routes, Route } from "react-router-dom";
import logo from './logo.svg';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

import Map from "./components/Map";

import {
  Navbar,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink,
  Button,
  Input,
  Row,
  Col,
  ListGroup,
  ListGroupItem,
  Form,
  InputGroup,
  Spinner
} from "reactstrap";
import Api from './api/api';
import { SearchResultDto } from './api';

function App() {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    setSelectedFile(file || null);
  };

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (selectedFile) {
      try {
        const response = await Api.upload({
          fileName: selectedFile.name,
          data: selectedFile
        });

        console.log(response);
        // Handle the response from the API
      } catch (error) {
        console.error(error);
      }
    }
  };

  const [searchQuery, setSearchQuery] = useState("");
  const [searchResults, setSearchResults] = useState(new SearchResultDto());

  const handleSearchChange = (event : any) => {
    setSearchQuery(event.target.value);
  };

  const handleSearchSubmit = async (event : any) => {
    event.preventDefault();

    try {
      const result = await Api.chat(searchQuery);
      setSearchResults(result);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="container-fluid">
      <Navbar color="light" light expand="md" fixed='top'>
        <NavbarBrand href="/" src={logo}><img
          alt="logo"
          src={logo}
          style={{
            height: 40,
            width: 40
          }}
        /></NavbarBrand>
        <Nav navbar className="me-auto">
          <NavItem>
            <NavLink href="/">RLake Proto</NavLink>
          </NavItem>
        </Nav>
        <Form onSubmit={handleSearchSubmit} className="d-flex">
        <InputGroup>
          <Input
            type="text"
            placeholder="Search..."
            value={searchQuery}
            onChange={handleSearchChange}
          />
          <Button type="submit" color="primary">
            {searchResults.items!.length === 0 ? "Search" : <Spinner size="sm" color="light" />}
          </Button>
        </InputGroup>
      </Form>
        <Form onSubmit={handleSubmit} className="d-flex">
          <InputGroup>
            <Input type="file" onChange={handleFileChange} placeholder="file..." />
            <Button type="submit" color="primary" disabled={!selectedFile}>
              Upload
            </Button>
          </InputGroup>
        </Form>
      </Navbar>
      <Row  style={{  paddingTop: 66 }}>
        <Col sm="3" md="2">
        <ListGroup>
          <ListGroupItem active>Items</ListGroupItem>
          {searchResults.items.map((result : any) => (
            <ListGroupItem key={result.id} tag="a" href={`/locations/${result.id}`}>
              {result.title}
              <br />
              {result.description}
            </ListGroupItem>
          ))}
        </ListGroup>
        </Col>
        <Col sm="9" md="10">
          <div style={{ height: "calc(100vh - 66px)", backgroundColor: "#eee" }}>
            <Routes >
              <Route path="/" element={<Map />}>
              </Route>
              <Route path="/*">
              </Route>
            </Routes >
          </div>
        </Col>
      </Row>
    </div>
  );
}

export default App;
