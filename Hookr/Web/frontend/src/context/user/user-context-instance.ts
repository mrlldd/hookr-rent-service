import { Role } from "../local-storage-utils";
import { createContext } from "react";
import { TelegramUser } from "telegram-login-button";

export type UserState = Omit<TelegramUser, "auth_date" | "hash">;

export interface LeveledUser extends UserState {
  role: Role;
}

export interface UserContext {
  state?: LeveledUser;
  dispatch: React.Dispatch<LeveledUser>;
}

export const UserContextInstance = createContext<UserContext>({
  dispatch: () => null,
});
