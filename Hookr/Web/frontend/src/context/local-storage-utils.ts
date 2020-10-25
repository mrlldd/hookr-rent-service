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

export type LocalStorageKeys = keyof JwtBundle;

export function saveJwtTokensToLocalStorage(jwtBundle: JwtInfo): void {
  localStorage.setItem("token", jwtBundle.token);
  localStorage.setItem("role", jwtBundle.role);
}

export function getFromLocalStorage(key: LocalStorageKeys): string | null {
  return localStorage.getItem(key);
}

export function setToLocalStorage(key: LocalStorageKeys, value: string): void {
  localStorage.setItem(key, value);
}

export function clearLocalStorage(): void {
  localStorage.clear();
}
