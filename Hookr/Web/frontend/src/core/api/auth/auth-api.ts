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
import { JwtInfo } from "../../../context/local-storage-utils";
import { TelegramUser } from "telegram-login-button";

const url = "api/auth";

const authPostMeta: RequestMeta = {
  url: url,
  method: HttpMethod.POST,
};

const refreshGetMeta: RequestMeta = {
  url: `${url}/refresh`,
  method: HttpMethod.GET,
};

type AuthPostBody = TelegramUser;

function authPostRequestFactory(body: AuthPostBody): ApiRequest<AuthPostBody> {
  return {
    ...authPostMeta,
    body: body,
  };
}

export function createSession(
  user: TelegramUser
): Promise<Success<AuthResult> | ErrorResponse> {
  return queryCall(authPostRequestFactory(user), true);
}

export function getRefreshToken(): Promise<Success<string> | ErrorResponse> {
  return queryCall(refreshGetMeta);
}

function refreshFactory(refreshToken: string, method: HttpMethod): RequestMeta {
  return {
    url: `${url}/refresh/${refreshToken}`,
    method,
  };
}

export interface AuthResult extends JwtInfo {
  user: TelegramUser;
  lifetime: number;
}

export function refreshSession(
  refreshToken: string
): Promise<Success<AuthResult> | ErrorResponse> {
  return queryCall(refreshFactory(refreshToken, HttpMethod.POST), true);
}

export function revokeToken(
  refreshToken: string
): Promise<EmptyResponse | ErrorResponse> {
  return commandCall(refreshFactory(refreshToken, HttpMethod.DELETE), true);
}
