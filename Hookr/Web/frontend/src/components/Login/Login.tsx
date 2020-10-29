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
import { deciderRoute } from "../../App";
import makeStyles from "@material-ui/core/styles/makeStyles";
import { createStyles, Theme } from "@material-ui/core";

const useStyles = makeStyles((theme: Theme) => {
  console.log(theme.palette.text.primary);
  return createStyles({
    title: {
      color: theme.palette.text.primary,
    },
  });
});

const Login: React.FC = () => {
  const [user, setter] = useState<TelegramUser>();
  const [state, dispatch] = useLoadableState(authenticate, false);
  useEffect(() => user && dispatch(user), [dispatch, user]);
  const styles = useStyles();
  return (
    <FullPageWrapper className="Login" data-testid="Login">
      <h1 className={styles.title}>Hookr</h1>
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
  if (!createSessionResult.success) {
    return createSessionResult as ErrorResponse;
  }

  const grabResult = await grabAndSaveAdditionalSessionDataAsync(
    unwrap(createSessionResult)
  );
  return successFactory(grabResult.success);
}

export default Login;
