import {
  ApiRequest,
  commandCall,
  EmptyResponse,
  ErrorResponse,
  HttpMethod,
  queryCall,
  RequestMeta,
  Success,
} from "../api-utils";
import { TelegramUser } from "@v9v/ts-react-telegram-login";
import { toDataCheckString } from "../../../utils";

const url = "api/auth";

const authPostMeta: RequestMeta = {
  url: url,
  method: HttpMethod.POST,
};

const authGetMeta: RequestMeta = {
  url: url,
  method: HttpMethod.GET,
};

type NoHashTelegramUser = Omit<TelegramUser, "hash">;

interface AuthPostBody {
  key: string;
  user: NoHashTelegramUser;
  hash: string;
}

function authPostRequestFactory(body: AuthPostBody): ApiRequest<AuthPostBody> {
  return {
    ...authPostMeta,
    body: body,
  };
}

function cloneWithoutHash(user: TelegramUser): NoHashTelegramUser {
  return {
    auth_date: user.auth_date,
    first_name: user.first_name,
    id: user.id,
    // @ts-ignore
    photo_url: user.photo_url,
    // @ts-ignore
    username: user.username,
  };
}

export function confirmTelegramLogin(
  user: TelegramUser
): Promise<EmptyResponse> {
  const clone = cloneWithoutHash(user);
  const key = toDataCheckString(clone);
  return commandCall(
    authPostRequestFactory({
      key,
      user,
      hash: user.hash + "s",
    })
  );
}

export function getTelegramUser(): Promise<Success<any> | ErrorResponse> {
  return queryCall(authGetMeta);
}
