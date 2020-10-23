import React, { useEffect, useState } from "react";
import "./Login.css";
import LoadingSpinner from "../LoadingSpinner/LoadingSpinner";
import TelegramLoginButton, {
  TelegramUser,
} from "@v9v/ts-react-telegram-login";
import { useLoadableState } from "../../context/store-utils";
import { confirmTelegramLogin } from "../../core/api/auth/auth-api";
import { saveJwtTokensToLocalStorage } from "../../context/local-storage-utils";

const Login: React.FC = () => {
  const [user, setter] = useState<TelegramUser>();
  const [state, dispatch] = useLoadableState(confirmTelegramLogin);
  useEffect(() => user && dispatch(user), [dispatch, user]);
  useEffect(
    () => state && state.data && saveJwtTokensToLocalStorage(state.data),
    [state]
  );
  return (
    <div className="Login" data-testid="Login">
      <h1>Hookr</h1>
      <div className="button-container">
        <LoadingSpinner loading={state.loading} size={35} />
        <div className={state.loading ? "opacity-" : ""}>
          <TelegramLoginButton
            dataOnAuth={setter}
            botName="mrlldd_development_bot"
            lang={navigator.languages ? navigator.languages[0] : "ru"} // todo global config state
          />
        </div>
      </div>
    </div>
  );
};

export default Login;
