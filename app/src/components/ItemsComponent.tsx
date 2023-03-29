import React, { useContext, useEffect } from 'react';

import {
  ListGroup,
  ListGroupItem
} from "reactstrap";
import { AppContext } from '../AppContext';


const ItemsComponent: React.FC = () => {
  const { state, updateState } = useContext(AppContext);

  return (
    <>
      <ListGroup>
        <ListGroupItem active>Conversations</ListGroupItem>
        {state.searchResult.items.map((result) => (
          <ListGroupItem key={result.id} tag="a" href={`/chat/${result.id}`}>
            {result.title}
          </ListGroupItem>
        ))}
      </ListGroup>
      <ListGroup className='my-2'>
        <ListGroupItem active>Points</ListGroupItem>
        {state.points.map((result) => (
          <ListGroupItem key={result.id} tag="a" href={`/point/${result.id}`}>
            {result.title}
          </ListGroupItem>
        ))}
      </ListGroup>
    </>
  );
};

export default ItemsComponent;
