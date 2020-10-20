import React, { useReducer } from "react";
import "./App.css";
import Login from "./components/Login/Login";
import {
  ErrorNotificatorContextInstance,
  errorNotificatorReducer,
} from "./context/error-notificator/error-notificator-context-instance";
import ErrorNotificator from "./components/ErrorNotificator/ErrorNotificator";

function App() {
  const [state, dispatch] = useReducer(errorNotificatorReducer, undefined);
  return (
    <ErrorNotificatorContextInstance.Provider
      value={{
        state: state,
        dispatch: dispatch,
      }}
    >
      <ErrorNotificator />
      <Login />
    </ErrorNotificatorContextInstance.Provider>
  );
}

export default App;
