import { Role } from "../local-storage-utils";
import { createContext } from "react";
import { TelegramUser } from "telegram-login-button";
import { nullString } from "../../core/utils";

export type UserState = Omit<TelegramUser, "auth_date" | "hash">;

export interface LeveledUser extends UserState {
  role: Role;
}

export interface UserContext {
  state: LeveledUser;
  dispatch: React.Dispatch<LeveledUser>;
}

export const userInitialState: LeveledUser = {
  role: Role.Default,
  first_name: nullString,
  id: 0,
  photo_url: nullString,
  username: nullString,
};

export const UserContextInstance = createContext<UserContext>({
  state: userInitialState,
  dispatch: () => null,
});
