import { useContext } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup, ListGroupItem, Navbar } from 'reactstrap';
import { AppContext } from '../AppContext';

const ConversationsComponent: React.FC = () => {
    const { state } = useContext(AppContext);
    const conversation = state.conversation;
    const post = conversation?.posts?.[0];

    return (
        <ListGroup style={{ height: '100%', overflowY: 'auto' }}>
            {conversation && post && (
                <>
                    <Navbar color='light' light sticky='top' className='px-2 border'>
                        <h4 className='1display-6 sticky-top'>{conversation.title}</h4>
                    </Navbar>
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
                                        className='list-group-item-action'
                                    >
                                        <h3 className='1display-6 text-bold'>{point.title}</h3>
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
    );
};

export default ConversationsComponent;
