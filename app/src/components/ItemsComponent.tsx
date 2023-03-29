import React, { useContext } from 'react';

import {
  Row,
  Col,
  ListGroup,
  ListGroupItem
} from "reactstrap";
import { AppContext } from '../appContext';


const ItemsComponent: React.FC = () => {
    const { state, setState } = useContext(AppContext);


    return (
    
        <ListGroup>
          <ListGroupItem active>Conversations</ListGroupItem>
          {state.searchResult.items.map((result : any) => (
            <ListGroupItem key={result.id} tag="a" href={`/locations/${result.id}`}>
              {result.title}
              <br />
              {result.description}
            </ListGroupItem>
          ))}
        </ListGroup>
    );
  };
  
  export default ItemsComponent;
  