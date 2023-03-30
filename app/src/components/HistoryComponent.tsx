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
        <div className='border pb-0 navbar navbar-light bg-light px-2 overflow-auto' ref={historyRef} style={{ maxHeight: '108px' }} >
            <div className='d-inline-flex align-items-stretch flex-wrap'>
                {conversations.map((conversation) => {
                    return (
                        <Link to={`/${conversation.id}`} key={conversation.id}
                            className="mx-1 text-truncate">
                            <h6>
                                <Badge className='rounded-pill bg-dark text-truncate'
                                    style={{ maxWidth: '20rem' }}>
                                    {conversation.title?.replace(/:$/, "")}
                                </Badge>
                            </h6>
                        </Link>
                    );
                })}
            </div>
        </div>
    );
};

export default HistoryComponent;
