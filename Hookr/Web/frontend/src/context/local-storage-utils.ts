import { TelegramUser } from "telegram-login-button";
import { LeveledUser } from "./user/user-context-instance";
import { AuthResult } from "../core/api/auth/auth-api";
import { Dispatch, useState } from "react";

export enum Role {
  Default = "default",
  Service = "service",
  Dev = "dev",
}

export interface JwtInfo {
  token: string;
  role: Role;
}

export interface JwtBundle extends JwtInfo {
  refresh: string;
}

export type LocalStorageKeys =
  | Exclude<keyof AuthResult, "user">
  | keyof JwtBundle
  | keyof TelegramUser
  | "expiresAt"
  | "theme";

export function saveJwtTokensToLocalStorage(jwtInfo: JwtInfo): void {
  localStorage.setItem("token", jwtInfo.token);
  localStorage.setItem("role", jwtInfo.role);
}

export function getFromLocalStorage(key: LocalStorageKeys): string | undefined {
  return localStorage.getItem(key) || undefined;
}

export function setToLocalStorage(key: LocalStorageKeys, value: string): void {
  localStorage.setItem(key, value);
}

export function clearLocalStorage(): void {
  localStorage.clear();
}

export function getJwtBundleFromLocalStorage(): Partial<JwtBundle> {
  return {
    refresh: getFromLocalStorage("refresh"),
    role: getFromLocalStorage("role") as Role,
    token: getFromLocalStorage("token"),
  };
}

const requiredKeys: LocalStorageKeys[] = [
  "id",
  "token",
  "role",
  "first_name",
  "refresh",
  "username",
  "photo_url",
  "lifetime",
];

export function localStorageHasNeededUserData(): boolean {
  return requiredKeys.map(getFromLocalStorage).every(Boolean);
}

export function getUserFromLocalStorage(): LeveledUser {
  return {
    role: getFromLocalStorage("role") as Role,
    first_name: getFromLocalStorage("first_name") || "[Unknown]",
    id: parseInt(getFromLocalStorage("id") || "0"),
    photo_url: getFromLocalStorage("photo_url") || "[Unknown]",
    username: getFromLocalStorage("username") || "[Unknown]",
  };
}

export function saveAuthResultToLocalStorage(authResult: AuthResult): void {
  Object.entries(authResult.user).forEach(([key, value]) =>
    setToLocalStorage(key as LocalStorageKeys, value)
  );
  setToLocalStorage("token", authResult.token);
  setToLocalStorage("role", authResult.role);
  setToLocalStorage("lifetime", authResult.lifetime.toString());
}

function ensureExistsInLocalStorage<T extends string>(
  key: LocalStorageKeys,
  instead: T
): T {
  const existing = getFromLocalStorage(key) as T;
  if (!existing) {
    setToLocalStorage(key, instead);
    return instead;
  }
  return existing;
}

export function useLocalStorageState<T extends string>(
  key: LocalStorageKeys,
  initial: T
): [T, Dispatch<T>] {
  const [state, setter] = useState<T>(ensureExistsInLocalStorage(key, initial));
  return [
    state,
    (x) => {
      setToLocalStorage(key, x);
      setter(x);
    },
  ];
}
