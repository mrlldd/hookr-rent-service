import React from "react";
import "./Login.css";
import LoadingSpinner from "../LoadingSpinner/LoadingSpinner";
import TelegramLoginButton from "@v9v/ts-react-telegram-login";
import { useLoadableState } from "../../context/store-utils";
import { confirmTelegramLogin } from "../../core/api/auth/auth-api";

const Login: React.FC = () => {
  const [state, dispatch] = useLoadableState(confirmTelegramLogin);
  return (
    <div className="Login" data-testid="Login">
      <h1>Hookr</h1>
      <div className="button-container">
        <LoadingSpinner loading={state.loading} size={35}>
          <TelegramLoginButton
            dataOnAuth={dispatch}
            botName="mrlldd_development_bot"
            lang={navigator.languages ? navigator.languages[0] : "ru"} // todo global config state
          />
        </LoadingSpinner>
      </div>
    </div>
  );
};

export default Login;
