import { TelegramUser } from "@v9v/ts-react-telegram-login";

export interface Action<T, P> {
  type: T;
  props: P;
}

export type NoHashTelegramUser = Omit<TelegramUser, "hash">;

export function omitHash(user: TelegramUser): NoHashTelegramUser {
  return { ...user };
}
