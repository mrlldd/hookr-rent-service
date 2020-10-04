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
import { omitHash } from "./store/utils";
import { cut, toDataCheckString } from "./utils";

const store = createStore(
  rootReducer,
  composeWithDevTools(applyMiddleware(thunk))
);

async function handler(user: TelegramUser) {
  store.dispatch<SetUserAction>({
    type: "[Auth] Set user",
    props: user,
  });
  console.log(omitHash(user));
  await fetch("/api/auth", {
    method: "post",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      key: toDataCheckString(cut(user, "hash")),
      hash: user.hash,
    }),
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
