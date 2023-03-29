import React, { useContext, useEffect } from 'react';
import { Link } from 'react-router-dom';

import {
  ListGroup,
  ListGroupItem
} from "reactstrap";
import { AppContext } from '../AppContext';


const ItemsComponent: React.FC = () => {
  const { state, updateState } = useContext(AppContext);

  return (
    <>
      <ListGroup className='my-2'>
        <ListGroupItem active>Points</ListGroupItem>
        {state.points.map((result) => (
          <ListGroupItem key={result.id} tag={Link} to={`/point/${result.id}`}>
            {result.title}
          </ListGroupItem>
        ))}
      </ListGroup>
    </>
  );
};

export default ItemsComponent;
