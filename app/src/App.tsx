import { useContext } from "react";
import { Routes, Route } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

import {
  Row,
  Col
} from "reactstrap";

import Map from "./components/MapComponent";
import NavbarComponent from "./components/NavbarComponent";

import FetchDataComponent from "./components/FetchDataComponent";
import ErrorAlert from "./components/ErrorAlert";
import { AppContext } from "./AppContext";
import ConversationsComponent from "./components/ConversationsComponent";

function App() {
  const { state } = useContext(AppContext);
  return (
    <div className="container-fluid" style={{ height: "100vh" }}>
      <NavbarComponent />
      <FetchDataComponent />
      {state.error &&
        <Row className="mx-1 mt-3 mb-1" >
          <ErrorAlert error={state.error} />
        </Row>
      }
      <Row>
        <Col sm="4" md="5" className="px-0" style={{ maxHeight: "calc(100vh - 66px)" }}>
          <ConversationsComponent />
        </Col>
        <Col sm="8" md="7" className="px-0">
          <div style={{ height: "calc(100vh - 66px)", backgroundColor: "#eee" }}>
            <Routes >
              <Route path="/" element={<Map />} />
              <Route path="/point/:id" element={<Map />} />
              <Route path="/conversation/:id" element={<Map />} />
            </Routes >
          </div>
        </Col>
      </Row>
    </div>
  );
}

export default App;
