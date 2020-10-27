import {
  saveAuthResultToLocalStorage,
  setToLocalStorage,
} from "../../../context/local-storage-utils";
import { EmptyResponse, ErrorResponse, Success } from "../api-utils";
import { AuthResult, getRefreshToken } from "./auth-api";

export async function grabAndSaveAdditionalSessionDataAsync(
  authResult: AuthResult
): Promise<EmptyResponse | ErrorResponse> {
  saveAuthResultToLocalStorage(authResult);
  const getRefreshTokenResult = await getRefreshToken();
  if (!getRefreshTokenResult.success) {
    return getRefreshTokenResult;
  }
  const refreshToken = (getRefreshTokenResult as Success<string>).data;
  setToLocalStorage("refresh", refreshToken);
  return {
    success: true,
  };
}
