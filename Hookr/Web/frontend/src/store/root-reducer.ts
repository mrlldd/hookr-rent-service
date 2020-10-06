import { authReducer } from "./auth/auth-reducer";
import { combineReducers } from "redux";
import { errorNotificatorReducer } from "./error-notificator/error-notificator-reducer";

export const rootReducer = combineReducers({
  auth: authReducer,
  errorNotificator: errorNotificatorReducer,
});

export type RootState = ReturnType<typeof rootReducer>;
