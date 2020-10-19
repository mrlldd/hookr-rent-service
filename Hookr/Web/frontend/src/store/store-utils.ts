import { Action } from "redux";
import { ThunkAction, ThunkDispatch } from "redux-thunk";
import { RootState } from "./root-reducer";
import { EmptyResponse, ErrorResponse, Success } from "../core/api/api-utils";
import { NotifyAboutErrorAction } from "./error-notificator/error-notificator-actions";
import { Response } from "../core/api/api-utils";

//state
export interface Loadable<T = undefined> {
  loading: boolean;
  data?: T;
}

export function emptyLoadable<T = undefined>(): Loadable<T> {
  return {
    loading: false,
  };
}

//actions

export type ActionType = string;

export type OutboundAction<T = undefined> = Loadable<T> & Action<ActionType>;

export type ActionProducer<T> = (payload: T) => OutboundAction<T>;

export interface RemoteActionBundle<T = void> {
  type: ActionType;
  request: OutboundAction<T>;
  success: ActionProducer<T>;
  error: OutboundAction<T>;
}

type RemoteActionStages = "Request" | "Success" | "Error";

function formatType(type: ActionType, stage: RemoteActionStages): ActionType {
  return `[${stage}] ${type}`;
}

function createRemoteActionsBundle<R = void>(
  type: ActionType
): RemoteActionBundle<R> {
  return {
    type: type,
    request: {
      type: formatType(type, "Request"),
      loading: true,
    },
    success: (data?: R) => ({
      type: formatType(type, "Success"),
      loading: false,
      data: data,
    }),
    error: {
      type: formatType(type, "Error"),
      loading: false,
    },
  };
}

type ThunkResult<T extends Action<ActionType>> = ThunkAction<
  Promise<T>,
  RootState,
  any,
  Action<ActionType>
>;

export type ThunkedDispatch<T> = ThunkDispatch<
  RootState,
  undefined,
  OutboundAction<T>
>;

export type ThunkOutboundAction<R = void> = ThunkResult<OutboundAction<R>>;

type RemoteActionProducer<R, T = void> = ((
  payload: T
) => ThunkOutboundAction<R>) &
  Action<ActionType>;

function internalCreateRemoteAction<R, C extends Response, T = undefined>(
  type: ActionType,
  asyncFunctor: (payload: T) => Promise<Success<R> | EmptyResponse>
): RemoteActionProducer<R | undefined, T> {
  const bundle = createRemoteActionsBundle<R | undefined>(type);
  const result = (payload: T) => (
    dispatch: ThunkDispatch<
      RootState,
      undefined,
      OutboundAction<R> | Action<ActionType>
    >
  ) => {
    dispatch(bundle.request);
    return asyncFunctor(payload).then((x) => {
      if (x.success) {
        return dispatch(bundle.success((x as Success<R>).data));
      }
      const dispatchedError = dispatch(bundle.error);
      dispatch(NotifyAboutErrorAction((x as unknown) as ErrorResponse));
      return dispatchedError;
    });
  };
  result.type = bundle.type;
  return result;
}

export function createRemoteCommand<T = undefined>(
  type: ActionType,
  asyncFunctor: (payload: T) => Promise<EmptyResponse>
): RemoteActionProducer<undefined, T> {
  return internalCreateRemoteAction(type, asyncFunctor);
}

export function createRemoteQuery<R, T = undefined>(
  type: ActionType,
  asyncFunctor: (payload: T) => Promise<Success<R> | ErrorResponse>
): RemoteActionProducer<R, T> {
  return internalCreateRemoteAction(type, asyncFunctor);
}

//reducers

export type RemoteActionReducer<S = undefined> = (
  state: Loadable<S>,
  action: OutboundAction<S>
) => Loadable<S>;

export function createRemoteActionReducer<S = undefined>(
  remoteAction: RemoteActionProducer<S, any>
): RemoteActionReducer<S> {
  const bundleType = remoteAction.type;
  return (state: Loadable<S>, action: OutboundAction<S>) => {
    switch (action.type) {
      case formatType(bundleType, "Request"):
      case formatType(bundleType, "Success"):
      case formatType(bundleType, "Error"): {
        return {
          loading: action.loading,
          data: action.data,
        };
      }
    }
    return state;
  };
}
