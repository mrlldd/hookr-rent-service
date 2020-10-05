import { Action } from "redux";
import { TelegramUser } from "@v9v/ts-react-telegram-login";

const SET_USER = "[Auth] Set user";

interface SetUserAction extends Action<typeof SET_USER> {
  user: TelegramUser;
}

export type AuthAction = SetUserAction;

export function SetUserAction(user: TelegramUser): AuthAction {
  return {
    type: SET_USER,
    user: user,
  };
}
