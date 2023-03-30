import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { Badge, ListGroup, ListGroupItem, Navbar } from 'reactstrap';
import { AppContext } from '../AppContext';

const LastConversationsComponent: React.FC = () => {
    const { state } = useContext(AppContext);

    const conversations = state.conversations;
    return (
        <Navbar color="light" light className='px-2 border'>
            <ListGroup>
                {conversations.map((conversation) => {
                    return (
                        <Link to={`/conversation/${conversation.id}`}>
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

export default LastConversationsComponent;
