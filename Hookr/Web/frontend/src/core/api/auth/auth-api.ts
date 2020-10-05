import { ApiRequest, HttpMethod, queryCall, RequestMeta } from "../api-utils";
import { TelegramUser } from "@v9v/ts-react-telegram-login";
import { cut, toDataCheckString } from "../../../utils";

const url = "api/auth";

const authPostMeta: RequestMeta = {
  url: url,
  method: HttpMethod.POST,
};

interface AuthPostBody {
  key: string;
  hash: string;
}

function authPostRequestFactory(body: AuthPostBody): ApiRequest<AuthPostBody> {
  return {
    ...authPostMeta,
    body: body,
  };
}

export function confirmTelegramLogin(user: TelegramUser): Promise<boolean> {
  const key = toDataCheckString(cut(user, "hash"));
  return queryCall(
    authPostRequestFactory({
      key,
      hash: user.hash,
    })
  );
}
