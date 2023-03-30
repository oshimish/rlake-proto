import React, { useContext, useLayoutEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import { Badge, ListGroup } from 'reactstrap';
import { AppContext } from '../AppContext';

const HistoryComponent: React.FC = () => {
    const { state, updateState } = useContext(AppContext);
    const historyRef = useRef<HTMLDivElement>(null);
    const conversations = state.conversations;

    useLayoutEffect(() => {
        if (historyRef.current) {
            updateState({ heightFix: historyRef.current.offsetHeight + historyRef.current.offsetTop });
        }
    }, [historyRef, updateState]);

    return (
        <div className='border pb-0 navbar navbar-light bg-light px-2' ref={historyRef} style={{ height: '88px' }} >
            <ListGroup>
                {conversations.map((conversation) => {
                    return (
                        <Link to={`/${conversation.id}`} key={conversation.id}>
                            <h6>
                                <Badge className='rounded-pill bg-dark'>
                                    {conversation.title?.replace(/:$/, "")}
                                </Badge>
                            </h6>
                        </Link>
                    );
                })}
            </ListGroup>
        </div>
    );
};

export default HistoryComponent;
