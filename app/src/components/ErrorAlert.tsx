import React from "react";
import { Alert } from "reactstrap";

interface ErrorAlertProps {
  error?: Error | null;
}

const ErrorAlert: React.FC<ErrorAlertProps> = ({ error }) => {
  if (!error) {
    return null;
  }

  return (
    <Alert color="danger" className="sticky-top"  >
      <h4 className="alert-heading">Error!</h4>
      <p>{error.message}</p>
    </Alert>
  );
};

export default ErrorAlert;
