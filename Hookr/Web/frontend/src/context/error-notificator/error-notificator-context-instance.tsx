import React, { createContext } from "react";
import { ErrorResponse } from "../../core/api/api-utils";
import { nullString } from "../../core/utils";

export type ErrorMessage = Omit<ErrorResponse, "success">;

export interface ErrorNotificatorContext {
  errorMessage: ErrorMessage;
  sendError: React.Dispatch<ErrorMessage>;
}

export const errorNotificatorInitialState: ErrorMessage = {
  type: nullString,
  description: nullString,
};

export const ErrorNotificatorContextInstance = createContext<
  ErrorNotificatorContext
>({
  errorMessage: errorNotificatorInitialState,
  sendError: () => undefined,
});
