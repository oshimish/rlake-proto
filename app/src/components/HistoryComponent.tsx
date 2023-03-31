import React, { useContext, useLayoutEffect, useRef } from 'react';
import { AzureMapsContext, IAzureMapsContextProps } from 'react-azure-maps';
import { Link } from 'react-router-dom';
import { Badge } from 'reactstrap';
import { AppContext } from '../AppContext';

const HistoryComponent: React.FC = () => {
    const { state, updateState } = useContext(AppContext);
    const historyRef = useRef<HTMLDivElement>(null);
    const { mapRef, isMapReady } = useContext<IAzureMapsContextProps>(AzureMapsContext);
    const conversations = state.conversations;

    useLayoutEffect(() => {
        const handleResize = () => {
            if (historyRef.current) {
                updateState({
                    heightFix: historyRef.current.offsetHeight
                });
            }
        };
        setTimeout(() => {
            handleResize();
        }, (1000));

        window.addEventListener("resize", handleResize);

        return () => {
            window.removeEventListener("resize", handleResize);
        };
    }, [historyRef, updateState, isMapReady]);

    if (conversations.length === 0) {
        return null;
    }

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
                                    {conversation.title?.replace(/:$/, "").replace(/^(Here are|There are)\s/, "")}
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
