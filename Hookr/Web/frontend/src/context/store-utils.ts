import { Dispatch, useCallback, useContext, useEffect, useState } from "react";
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
  const [paramsState, setParams] = useState<P>();
  const errorContext = useContext(ErrorNotificatorContextInstance);
  useEffect(() => {
    if (paramsState) {
      setter({
        loading: true,
      });
      asyncFunctor(paramsState).then((x) => {
        if (!x.success) {
          errorContext.dispatch({
            type: "[Error Notificator] Notify",
            message: x as ErrorResponse,
          });
        }
        return setter({
          loading: false,
          data: (x as Success<T>).data,
        });
      });
    }
  }, [paramsState, asyncFunctor]);
  return [state, setParams];
}
