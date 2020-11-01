import React, { useEffect, useState } from "react";
import "./App.css";
import Login from "./components/Login/Login";
import {
  ErrorMessage,
  ErrorNotificatorContextInstance,
  errorNotificatorInitialState,
} from "./context/error-notificator/error-notificator-context-instance";
import ErrorNotificator from "./components/ErrorNotificator/ErrorNotificator";
import { BrowserRouter, Redirect, Route } from "react-router-dom";
import Decider from "./components/Decider/Decider";
import {
  LeveledUser,
  UserContextInstance,
  userInitialState,
} from "./context/user/user-context-instance";
import LoggedInWrapper from "./components/LoggedInWrapper/LoggedInWrapper";
import { nullString } from "./core/utils";
import { ThemeProvider } from "@material-ui/core/styles";
import { defaultTheme, getTheme, Themes } from "./core/themes";
import { useLocalStorageState } from "./context/local-storage-utils";
import { AppThemeContext } from "./context/themes/app-theme-context";

export const loginRoute = "/login";
export const dashboardRoute = "/dashboard";
export const deciderRoute = "/decide";
export const settingsRoute = "/settings";

function App() {
  const [theme, themeSetter] = useLocalStorageState<Themes>(
    "theme",
    defaultTheme
  );
  const [state, setter] = useState<ErrorMessage>(errorNotificatorInitialState);
  const [userState, userSetter] = useState<LeveledUser>(userInitialState);
  useEffect(() => {
    if (userState.photo_url === nullString) {
      return;
    }
    new Image().src = userState.photo_url;
    // just to trigger avatar load (and cache later)
  }, [userState]);
  return (
    <AppThemeContext.Provider
      value={{
        theme: theme,
        setTheme: themeSetter,
      }}
    >
      <ThemeProvider theme={getTheme(theme)}>
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
              <Route path={dashboardRoute} component={LoggedInWrapper} />
              <Redirect from="/" to={deciderRoute} />
              {userState === userInitialState && <Redirect to={deciderRoute} />}
            </BrowserRouter>
          </UserContextInstance.Provider>
        </ErrorNotificatorContextInstance.Provider>
      </ThemeProvider>
    </AppThemeContext.Provider>
  );
}

export default App;
