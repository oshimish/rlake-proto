import React, { useEffect, useState } from "react";
import { Alert } from "reactstrap";

interface ErrorAlertProps {
  error?: Error | null;
}

const ErrorAlert: React.FC<ErrorAlertProps> = ({ error }) => {

  const [visible, setVisible] = useState(true);
  const onDismiss = () => setVisible(false);

  useEffect(() => {
    if (!error) setVisible(true);
  }, [error])

  if (!error) {
    return null;
  }

  return (
    <Alert color="danger" className="sticky-top" isOpen={visible && error !== null} toggle={onDismiss} >
      <h4 className="alert-heading">Error!</h4>
      <p>{error.message}</p>
    </Alert>
  );
};

export default ErrorAlert;
