import { useContext, useEffect } from "react";
import { Routes, Route, useParams } from "react-router-dom";
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
import HistoryComponent from "./components/HistoryComponent";

function AppRoute() {
  const { state, updateState } = useContext(AppContext);
  const { id, conversationId } = useParams<{ id: string, conversationId?: string }>();

  useEffect(() => {
    const point = state.points.find((p) => p.id === id);
    updateState({ point });
  }, [id, state.points, updateState]);

  useEffect(() => {
    if (state.conversation?.posts) {
      const post = state.conversation?.posts[0];
      const point = post.points![0];
      updateState({ point, points: post.points });
    }
  }, [state.conversation, updateState]);

  useEffect(() => {
    const conversation = state.conversations.find((p) => p.id === conversationId);
    if (conversation) {
      updateState({
        conversation
      });
    }
  }, [conversationId, state.conversations, updateState]);

  return (
    <Row>
      <Col sm="4" md="5" className="px-0" style={{ maxHeight: "calc(100vh - 66px)" }}>
        <ConversationsComponent />
      </Col>
      <Col sm="8" md="7" className="px-0">
        <div style={{ height: "calc(100vh - 66px)", backgroundColor: "#eee" }}>
          <Map />
        </div>
      </Col>
    </Row>
  );
}

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
      <HistoryComponent />
      <Routes>
        <Route path="/" element={<AppRoute />} />
        <Route path="/:conversationId" element={<AppRoute />} />
        <Route path="/:conversationId/:id" element={<AppRoute />} />
      </Routes >

    </div>
  );
}

export default App;
