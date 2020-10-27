import React, { useState } from "react";
import "./App.css";
import Login from "./components/Login/Login";
import {
  ErrorMessage,
  ErrorNotificatorContextInstance,
} from "./context/error-notificator/error-notificator-context-instance";
import ErrorNotificator from "./components/ErrorNotificator/ErrorNotificator";
import { BrowserRouter, Redirect, Route } from "react-router-dom";
import Decider from "./components/Decider/Decider";
import {
  LeveledUser,
  UserContextInstance,
} from "./context/user/user-context-instance";
import Dashboard from "./components/Dashboard/Dashboard";

export const loginRoute = "/login";
export const dashboardRoute = "/dashboard";
export const deciderRoute = "/decide";

function App() {
  const [state, setter] = useState<ErrorMessage | undefined>(undefined);
  const [userState, userSetter] = useState<LeveledUser | undefined>(undefined);
  return (
    <ErrorNotificatorContextInstance.Provider
      value={{
        errorMessage: state,
        sendError: setter,
      }}
    >
      <ErrorNotificator />
      <UserContextInstance.Provider
        value={{
          state: userState,
          dispatch: userSetter,
        }}
      >
        <BrowserRouter>
          <Route path={deciderRoute} component={Decider} />
          <Route path={loginRoute} component={Login} />
          <Route path={dashboardRoute} component={Dashboard} />
          <Redirect from="/" to={deciderRoute} />
        </BrowserRouter>
      </UserContextInstance.Provider>
    </ErrorNotificatorContextInstance.Provider>
  );
}

export default App;
