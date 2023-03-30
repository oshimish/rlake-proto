import { Routes, Route } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

import {
  Row,
  Col
} from "reactstrap";

import Map from "./components/MapComponent";
import NavbarComponent from "./components/NavbarComponent";
import ItemsComponent from "./components/ItemsComponent";

import FetchDataComponent from "./components/FetchDataComponent";

function App() {
  return (
    <div className="container-fluid">
      <NavbarComponent />
      <Row className="mx-1 mt-3 mb-1" style={{ paddingTop: 66 }}>
        <FetchDataComponent />
      </Row>
      <Row className='mt-1'>
        <Col sm="3" md="2">
          <ItemsComponent />
        </Col>
        <Col sm="9" md="10">
          <div style={{ height: "calc(100vh - 66px)", backgroundColor: "#eee" }}>
            <Routes >
              <Route path="/" element={<Map />} />
              <Route path="/point/:id" element={<Map />} />
              <Route path="/chat/:id" element={<Map />} />
            </Routes >
          </div>
        </Col>
      </Row>
    </div>
  );
}

export default App;
