import React from "react";
import "./App.css";
import TelegramLoginButton, {
  TelegramUser,
} from "@v9v/ts-react-telegram-login";
import { SetUserAction } from "./store/auth/auth-actions";
import ErrorNotificator from "./components/error-notificator/ErrorNotificator";
import { store } from "./store/store";
import { confirmTelegramLogin } from "./core/api/auth/auth-api";

async function handler(user: TelegramUser) {
  store.dispatch(SetUserAction(user));
  console.log(await confirmTelegramLogin(user));
}

function App() {
  return (
    <ErrorNotificator>
      <div className="login-container">
        <h1>Hookr</h1>
        <TelegramLoginButton
          dataOnAuth={handler}
          botName="mrlldd_development_bot"
          lang={navigator.languages ? navigator.languages[0] : "ru"} // todo global config state
        />
      </div>
    </ErrorNotificator>
  );
}

export default App;
