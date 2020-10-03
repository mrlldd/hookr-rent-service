import React from "react";
import "./App.css";
import TelegramLoginButton, {
  TelegramUser,
} from "@v9v/ts-react-telegram-login";

function handler(user: TelegramUser): void {
  console.log(user);
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
