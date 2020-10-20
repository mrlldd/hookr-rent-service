import React, { createContext } from "react";
import { ErrorResponse } from "../../core/api/api-utils";
import { ErrorNotificatorAction } from "./error-notificator-actions";

export interface ErrorNotificatorContext {
  state?: ErrorResponse;
  dispatch: React.Dispatch<ErrorNotificatorAction>;
}

export const ErrorNotificatorContextInstance = createContext<
  ErrorNotificatorContext
>({
  state: undefined,
  dispatch: () => null,
});

export function errorNotificatorReducer(
  state: ErrorResponse | undefined,
  action: ErrorNotificatorAction
): ErrorResponse | undefined {
  if (!action) {
    return undefined;
  }
  switch (action.type) {
    case "[Error Notificator] Notify":
      return { ...action.message };
  }
  return state;
}
