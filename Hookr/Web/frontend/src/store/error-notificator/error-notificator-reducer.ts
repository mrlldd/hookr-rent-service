import { ErrorResponse } from "../../core/api/api-utils";
import { ErrorNotificatorAction } from "./error-notificator-actions";

export interface ErrorNotificatorState extends Partial<ErrorResponse> {
  opened: boolean;
  fullSize: boolean;
}

const initialState: ErrorNotificatorState = {
  opened: false,
  fullSize: false,
};

export function errorNotificatorReducer(
  state: ErrorNotificatorState = initialState,
  action: ErrorNotificatorAction
): ErrorNotificatorState {
  switch (action.type) {
    case "[Error Notificator] Notify":
      return { ...action.message, opened: true, fullSize: state.fullSize };
    case "[Error Notificator] Hide":
      return { ...state, opened: false };
    case "[Error Notificator] Change size":
      return { ...state, fullSize: !state.fullSize };
    case "[Error Notificator] Reset":
      return { ...state, ...initialState };
  }
  return state;
}
