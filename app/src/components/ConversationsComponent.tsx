import { useContext } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup, ListGroupItem } from 'reactstrap';
import { AppContext } from '../AppContext';

const ConversationsComponent: React.FC = () => {
    const { state, updateState } = useContext(AppContext);

    const conversation = state.conversation;
    const post = conversation?.posts?.[0];

    return (
        <>
            <ListGroup style={{ height: `calc(100vh - ${state.navFix}px)`, overflowY: 'auto' }}>
                {conversation && post && (
                    <>
                        <ListGroupItem
                            key={conversation.id}
                            action
                            className='px-2 border-top-0'
                        >
                            {post.points && (
                                <ListGroup style={{ marginTop: 10 }}>
                                    {post.points.map((point) => (
                                        <ListGroupItem
                                            key={point.id}
                                            tag={Link}
                                            tooltip={point.additionalInfo}
                                            to={`/${conversation.id}/${point.id}`}
                                            className='list-group-item-action p-4'
                                        >
                                            <h3 className='text-bold'>{point.title}</h3>
                                            <p className='lead'>{point.reason}</p>
                                            <p className='mb-1'>{point.description}</p>
                                            <small className='text-muted'>{point.additionalInfo}</small>
                                        </ListGroupItem>
                                    ))}
                                </ListGroup>
                            )}
                        </ListGroupItem>
                    </>
                )}
            </ListGroup>
        </>
    );
};

export default ConversationsComponent;
