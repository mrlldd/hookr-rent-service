import React, { useState } from "react";
import "./App.css";
import Login from "./components/Login/Login";
import {
  ErrorMessage,
  ErrorNotificatorContextInstance,
} from "./context/error-notificator/error-notificator-context-instance";
import ErrorNotificator from "./components/ErrorNotificator/ErrorNotificator";

function App() {
  const [state, setter] = useState<ErrorMessage | undefined>(undefined);
  return (
    <ErrorNotificatorContextInstance.Provider
      value={{
        errorMessage: state,
        sendError: setter,
      }}
    >
      <ErrorNotificator />
      <Login />
    </ErrorNotificatorContextInstance.Provider>
  );
}

export default App;
