import { TelegramUser } from "@v9v/ts-react-telegram-login";
import { Action } from "../utils";

export type AuthActionType = "[Auth] Set user" | "Another Action";

export type AuthAction<T> = Action<AuthActionType, T>;

export interface SetUserAction extends AuthAction<TelegramUser> {
  type: "[Auth] Set user";
}
