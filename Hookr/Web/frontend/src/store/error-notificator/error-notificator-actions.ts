import { ErrorResponse } from "../../core/api/api-utils";
import { Action } from "redux";

const NOTIFY_ABOUT_ERROR = "[Error Notificator] Notify";
const HIDE_NOTIFICATOR = "[Error Notificator] Hide";
const NOTIFICATOR_CHANGE_SIZE = "[Error Notificator] Change size";
const RESET_NOTIFICATOR = "[Error Notificator] Reset";

interface NotifyAboutErrorAction extends Action<typeof NOTIFY_ABOUT_ERROR> {
  message: ErrorResponse;
}

export type ErrorNotificatorAction =
  | NotifyAboutErrorAction
  | Action<typeof HIDE_NOTIFICATOR>
  | Action<typeof NOTIFICATOR_CHANGE_SIZE>
  | Action<typeof RESET_NOTIFICATOR>;

export function NotifyAboutErrorAction(
  error: ErrorResponse
): ErrorNotificatorAction {
  return {
    type: NOTIFY_ABOUT_ERROR,
    message: error,
  };
}

export const HideNotificatorAction: ErrorNotificatorAction = {
  type: HIDE_NOTIFICATOR,
};

export const ChangeNotificatorSizeAction: ErrorNotificatorAction = {
  type: NOTIFICATOR_CHANGE_SIZE,
};

export const ResetNotificatorAction: ErrorNotificatorAction = {
  type: RESET_NOTIFICATOR,
};
