import React, { useEffect, useState } from "react";
import "./Login.css";
import LoadingSpinner from "../LoadingSpinner/LoadingSpinner";
import TelegramLoginButton, {
  TelegramUser,
} from "@v9v/ts-react-telegram-login";
import { useLoadableState } from "../../context/store-utils";
import {
  createSession,
  getRefreshToken,
  revokeToken,
} from "../../core/api/auth/auth-api";
import {
  clearLocalStorage,
  getFromLocalStorage,
  JwtInfo,
  saveJwtTokensToLocalStorage,
  setToLocalStorage,
} from "../../context/local-storage-utils";
import {
  EmptyResponse,
  ErrorResponse,
  Success,
} from "../../core/api/api-utils";

const Login: React.FC = () => {
  const [user, setter] = useState<TelegramUser>();
  const [state, dispatch] = useLoadableState(authenticate);
  useEffect(() => user && dispatch(user), [dispatch, user]);
  return (
    <div className="Login" data-testid="Login">
      <h1>Hookr</h1>
      <div>
        <LoadingSpinner loading={state.loading} size={35}>
          <TelegramLoginButton
            buttonSize={"large"}
            dataOnAuth={setter}
            botName="mrlldd_development_bot"
            lang={navigator.languages ? navigator.languages[0] : "ru"} // todo global config state
          />
        </LoadingSpinner>
      </div>
    </div>
  );
};

async function authenticate(
  user: TelegramUser
): Promise<EmptyResponse | ErrorResponse> {
  const existingRefreshToken = getFromLocalStorage("refresh");
  clearLocalStorage();
  const [createSessionResult] = await Promise.all([
    createSession(user),
    existingRefreshToken
      ? revokeToken(existingRefreshToken)
      : Promise.resolve<EmptyResponse>({
          success: true,
        }),
  ]);
  if (!createSessionResult.success) {
    return createSessionResult;
  }
  const jwtInfo = (createSessionResult as Success<JwtInfo>).data;
  saveJwtTokensToLocalStorage(jwtInfo);
  const getRefreshTokenResult = await getRefreshToken();
  if (!getRefreshTokenResult.success) {
    return getRefreshTokenResult;
  }
  const refreshToken = (getRefreshTokenResult as Success<string>).data;
  setToLocalStorage("refresh", refreshToken);
  return {
    success: true,
  };
}

export default Login;
