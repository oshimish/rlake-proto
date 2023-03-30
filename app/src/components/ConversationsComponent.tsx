import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { ListGroup, ListGroupItem, Navbar } from 'reactstrap';
import { AppContext } from '../AppContext';

const ConversationsComponent: React.FC = () => {
    const { state } = useContext(AppContext);

    const conversations = state.conversations.slice(0, 1);
    return (
        <ListGroup style={{ height: '100%', overflowY: 'auto' }}>
            {conversations.map((conversation) => {
                const { id, title } = conversation;
                const post = conversation.posts?.[0];

                if (!post) {
                    return null;
                }

                return (
                    <>
                        <Navbar color="light" light sticky='top' className='px-2 border'>
                            <h4 className="1display-6 sticky-top">{title}</h4>
                        </Navbar>
                        <ListGroupItem key={id} action className='px-2 border-top-0'>
                            {/* <div>{post.text}</div> */}
                            {post.points && (
                                <ListGroup style={{ marginTop: 10 }}>
                                    {post.points.map((point) => (
                                        <ListGroupItem
                                            key={point.id}
                                            tag={Link}
                                            tooltip={point.additionalInfo}
                                            to={`/point/${point.id}`}
                                            className="list-group-item-action"
                                        >
                                            <h3 className="1display-6 text-bold">{point.title}</h3>
                                            <p className="lead">{point.reason}</p>
                                            <p className="mb-1">{point.description}</p>
                                            <small className="text-muted">{point.additionalInfo}</small>
                                            {/* <span className="badge bg-primary rounded-pill">{post.points?.length}</span> */}
                                        </ListGroupItem>
                                    ))}
                                </ListGroup>
                            )}
                        </ListGroupItem>
                    </>
                );
            })}
        </ListGroup>
    );
};

export default ConversationsComponent;
