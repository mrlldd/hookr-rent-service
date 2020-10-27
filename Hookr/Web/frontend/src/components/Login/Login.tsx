import React, { useEffect, useState } from "react";
import "./Login.css";
import LoadingSpinner from "../LoadingSpinner/LoadingSpinner";
import { useLoadableState } from "../../context/store-utils";
import {
  AuthResult,
  createSession,
  revokeToken,
} from "../../core/api/auth/auth-api";
import {
  clearLocalStorage,
  getFromLocalStorage,
  JwtInfo,
} from "../../context/local-storage-utils";
import {
  ErrorResponse,
  Success,
  successFactory,
  unwrap,
} from "../../core/api/api-utils";
import { grabAndSaveAdditionalSessionDataAsync } from "../../core/api/auth/auth-api-utils";
import FullPageWrapper from "../FullPageWrapper/FullPageWrapper";
import TelegramLoginButton, { TelegramUser } from "telegram-login-button";
import { Redirect } from "react-router";
import { dashboardRoute, deciderRoute } from "../../App";

const Login: React.FC = () => {
  const [user, setter] = useState<TelegramUser>();
  const [state, dispatch] = useLoadableState(authenticate, false);
  useEffect(() => user && dispatch(user), [dispatch, user]);
  return (
    <FullPageWrapper className="Login" data-testid="Login">
      <h1>Hookr</h1>
      {state.data ? (
        <Redirect to={deciderRoute} />
      ) : (
        <div>
          <LoadingSpinner loading={state.loading} size={35}>
            <TelegramLoginButton
              buttonSize={"large"}
              usePic={true}
              dataOnauth={setter}
              botName="mrlldd_development_bot"
            />
          </LoadingSpinner>
        </div>
      )}
    </FullPageWrapper>
  );
};

async function authenticate(
  user: TelegramUser
): Promise<Success<boolean> | ErrorResponse> {
  const existingRefreshToken = getFromLocalStorage("refresh");
  if (existingRefreshToken) {
    await revokeToken(existingRefreshToken);
  }
  clearLocalStorage();
  const createSessionResult = await createSession(user);
  if (!createSessionResult) {
    return successFactory(false);
  }

  const grabResult = await grabAndSaveAdditionalSessionDataAsync(
    unwrap(createSessionResult)
  );
  return successFactory(grabResult.success);
}

export default Login;
