import React, { useContext, useEffect } from "react";
import "./Decider.css";
import FullPageWrapper from "../FullPageWrapper/FullPageWrapper";
import LoadingSpinner from "../LoadingSpinner/LoadingSpinner";
import {
  UserContext,
  UserContextInstance,
  userInitialState,
} from "../../context/user/user-context-instance";
import { useLoadableState } from "../../context/store-utils";
import {
  AuthResult,
  getRefreshToken,
  refreshSession,
} from "../../core/api/auth/auth-api";
import {
  getFromLocalStorage,
  getUserFromLocalStorage,
  localStorageHasNeededUserData,
  saveAuthResultToLocalStorage,
  setToLocalStorage,
} from "../../context/local-storage-utils";
import {
  ErrorResponse,
  Success,
  successFactory,
  unwrap,
} from "../../core/api/api-utils";
import { Redirect } from "react-router";
import { dashboardRoute, loginRoute } from "../../App";

const Decider: React.FC = () => {
  const context = useContext(UserContextInstance);
  const [decided, load] = useLoadableState(decide, undefined);
  useEffect(() => {
    load(context);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
  return (
    <FullPageWrapper className="Decider" data-testid="Decider">
      {decided.data === undefined ? (
        <LoadingSpinner size={100} loading={decided.loading} />
      ) : (
        <Redirect to={decided.data ? dashboardRoute : loginRoute} />
      )}
    </FullPageWrapper>
  );
};

async function decide(
  context: UserContext
): Promise<Success<boolean> | ErrorResponse> {
  if (context.state !== userInitialState) {
    return successFactory(true);
  }
  if (localStorageHasNeededUserData()) {
    const user = getUserFromLocalStorage();
    context.dispatch(user);
    return successFactory(true);
  }

  const refreshToken = getFromLocalStorage("refresh");
  if (!refreshToken) {
    return successFactory(false);
  }

  const sessionResponse = await refreshSession(refreshToken);
  if (!sessionResponse.success) {
    return successFactory(false);
  }
  const session: AuthResult = unwrap(sessionResponse);
  saveAuthResultToLocalStorage(session);
  const newRefreshToken = await getRefreshToken();
  if (newRefreshToken.success) {
    setToLocalStorage("refresh", unwrap(newRefreshToken));
  }
  const user = session.user;
  context.dispatch({
    username: user.username,
    photo_url: user.photo_url,
    id: user.id,
    first_name: user.first_name,
    role: session.role,
  });
  return successFactory(true);
}

export default Decider;
