import { TelegramUser } from "@v9v/ts-react-telegram-login";
import { AuthAction } from "./auth-actions";

export type UserState = TelegramUser;

const initialState: UserState = {
  last_name: "",
  id: 0,
  hash: "",
  first_name: "",
  auth_date: 0,
};

export function authReducer(
  state: UserState = initialState,
  action: AuthAction<TelegramUser>
): UserState {
  switch (action.type) {
    case "[Auth] Set user": {
      return { ...action.props };
    }
  }
  return state;
}
