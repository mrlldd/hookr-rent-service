import {
  ApiRequest,
  EmptyResponse,
  ErrorResponse,
  HttpMethod,
  queryCall,
  RequestMeta,
  Success,
} from "../api-utils";
import { TelegramUser } from "@v9v/ts-react-telegram-login";
import { JwtInfo } from "../../../context/local-storage-utils";

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
): Promise<Success<JwtInfo> | ErrorResponse> {
  return queryCall(authPostRequestFactory(user));
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

export function refreshSession(
  refreshToken: string
): Promise<Success<JwtInfo> | ErrorResponse> {
  return queryCall(refreshFactory(refreshToken, HttpMethod.POST));
}

export function revokeToken(
  refreshToken: string
): Promise<EmptyResponse | ErrorResponse> {
  return queryCall(refreshFactory(refreshToken, HttpMethod.DELETE));
}
