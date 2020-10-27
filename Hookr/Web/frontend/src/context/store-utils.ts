import { Dispatch, useContext, useEffect, useState } from "react";
import { ErrorNotificatorContextInstance } from "./error-notificator/error-notificator-context-instance";
import { EmptyResponse, ErrorResponse, Success } from "../core/api/api-utils";

export interface Loadable<T> {
  data?: T;
  loading: boolean;
}

export function useLoadableState<P = void, T = void>(
  asyncFunctor: (payload: P) => Promise<Success<T> | EmptyResponse>,
  initialState?: T
): [Loadable<T>, Dispatch<P>] {
  const [state, setter] = useState<Loadable<T>>({
    loading: false,
    data: initialState,
  });
  const [paramsState, paramsSetter] = useState<P | null>(null);
  const { sendError } = useContext(ErrorNotificatorContextInstance);
  useEffect(() => {
    if (paramsState === null) {
      return;
    }
    setter({
      loading: true,
    });
    asyncFunctor(paramsState).then((x) => {
      if (!x.success) {
        sendError(x as ErrorResponse);
      }
      return setter({
        loading: false,
        data: (x as Success<T>).data,
      });
    });
  }, [paramsState, asyncFunctor, sendError]);
  return [state, paramsSetter];
}

export function useSwitchState(
  initialState?: boolean
): [boolean, () => void, (value: boolean) => void] {
  const [state, setter] = useState(initialState || false);

  function switcher(): void {
    setter(!state);
  }

  return [state, switcher, setter];
}
