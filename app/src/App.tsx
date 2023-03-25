import React, { useState } from 'react';
import logo from './logo.svg';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import {AzureMap, AzureMapsProvider, IAzureMapOptions} from 'react-azure-maps'
import {AuthenticationType} from 'azure-maps-control'

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
  ListGroupItem
} from "reactstrap";

// function App() {
//   return (
//     <div className="App">
//       <header className="App-header">
//         <img src={logo} className="App-logo" alt="logo" />
//         <p>
//           Edit <code>src/App.tsx</code> and save to reload.
//         </p>
//         <a
//           className="App-link"
//           href="https://reactjs.org"
//           target="_blank"
//           rel="noopener noreferrer"
//         >
//           Learn React
//         </a>
//       </header>
//     </div>
//   );
// }


const option: IAzureMapOptions = {
    authOptions: {
        authType: AuthenticationType.subscriptionKey,
        subscriptionKey: process.env.REACT_APP_MAPS_KEY ?? '' // Your subscription key
    },
}


function App() {
  const [username, setUsername] = useState("");



  const handleUploadClick = () => {
    console.log("Upload clicked");
  };

  return (
    <AzureMapsProvider>
    <div className="container-fluid">
      <Navbar color="light" light expand="md">
        <NavbarBrand href="/" src={logo}>Logo</NavbarBrand>
        <Nav className="mr-auto" navbar>
          <NavItem>
            <NavLink href="#">Upload</NavLink>
          </NavItem>
        </Nav>
        {/* <Input
          type="text"
          placeholder="Username"
          value={username}
        /> */}
        <Button color="primary" onClick={handleUploadClick}>
          Upload
        </Button>
      </Navbar>
      <Row>
        <Col sm="3">
          <ListGroup>
            <ListGroupItem active>Pinned Items</ListGroupItem>
            <ListGroupItem>Item 1</ListGroupItem>
            <ListGroupItem>Item 2</ListGroupItem>
            <ListGroupItem>Item 3</ListGroupItem>
          </ListGroup>
        </Col>
        <Col sm="9">
          <div style={{ height: "100vh", backgroundColor: "#eee" }}>
            <AzureMap options={option} />
          </div>
        </Col>
      </Row>
    </div>
  </AzureMapsProvider>
  );
}

export default App;
