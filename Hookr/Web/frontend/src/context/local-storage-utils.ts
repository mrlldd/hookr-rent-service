export interface JwtTokens {
  token: string;
  refresh: string;
}

export type LocalStorageKeys = keyof JwtTokens;

export function saveJwtTokensToLocalStorage(tokens: JwtTokens): void {
  localStorage.setItem("token", tokens.token);
  localStorage.setItem("refresh", tokens.refresh);
}

export function getFromLocalStorage(key: LocalStorageKeys): string | null {
  return localStorage.getItem(key);
}

export function setToLocalStorage(key: LocalStorageKeys, value: string): void {
  localStorage.setItem(key, value);
}
