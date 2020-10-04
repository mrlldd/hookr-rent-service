import React from "react";
import "./App.css";
import TelegramLoginButton, {
  TelegramUser,
} from "@v9v/ts-react-telegram-login";
import { createStore, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import { SetUserAction } from "./store/auth/auth-actions";
import { composeWithDevTools } from "redux-devtools-extension";
import { rootReducer } from "./store/root-reducer";
const store = createStore(
  rootReducer,
  composeWithDevTools(applyMiddleware(thunk))
);

function handler(user: TelegramUser): void {
  store.dispatch<SetUserAction>({
    type: "[Auth] Set user",
    props: user,
  });
}

function App() {
  return (
    <div className="login-container">
      <h1>Hookr</h1>
      <TelegramLoginButton
        dataOnAuth={handler}
        botName="mrlldd_development_bot"
        lang={navigator.languages ? navigator.languages[0] : "ru"} // todo global config state
      />
    </div>
  );
}

export default App;
