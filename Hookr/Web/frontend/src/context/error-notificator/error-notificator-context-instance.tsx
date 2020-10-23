import React, { createContext } from "react";
import { ErrorResponse } from "../../core/api/api-utils";

export type ErrorMessage = Omit<ErrorResponse, "traceId" & "success">;

export interface ErrorNotificatorContext {
  errorMessage?: ErrorMessage;
  sendError: React.Dispatch<ErrorMessage>;
}

export const ErrorNotificatorContextInstance = createContext<
  ErrorNotificatorContext
>({
  errorMessage: undefined,
  sendError: () => undefined,
});
