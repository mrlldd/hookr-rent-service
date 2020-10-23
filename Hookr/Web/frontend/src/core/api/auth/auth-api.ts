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
import { JwtTokens } from "../../../context/local-storage-utils";

const url = "api/auth";

const authPostMeta: RequestMeta = {
  url: url,
  method: HttpMethod.POST,
};

const authGetMeta: RequestMeta = {
  url: url,
  method: HttpMethod.GET,
};

type AuthPostBody = TelegramUser;

function authPostRequestFactory(body: AuthPostBody): ApiRequest<AuthPostBody> {
  return {
    ...authPostMeta,
    body: body,
  };
}

export function confirmTelegramLogin(
  user: TelegramUser
): Promise<Success<JwtTokens> | ErrorResponse> {
  return queryCall(authPostRequestFactory(user));
}
