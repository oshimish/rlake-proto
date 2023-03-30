import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { Badge, ListGroup, Navbar } from 'reactstrap';
import { AppContext } from '../AppContext';

const HistoryComponent: React.FC = () => {
    const { state } = useContext(AppContext);

    const conversations = state.conversations;
    return (
        <Navbar color="light" light className='border'>
            <ListGroup>
                {conversations.map((conversation) => {
                    return (
                        <Link to={`/${conversation.id}`} key={conversation.id}>
                            <h6>
                                <Badge className='rounded-pill bg-dark'>
                                    {conversation.title}
                                </Badge>
                            </h6>
                        </Link>
                    );
                })}
            </ListGroup>
        </Navbar>
    );
};

export default HistoryComponent;
