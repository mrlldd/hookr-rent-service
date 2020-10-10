import { store } from "../../store/store";
import { NotifyAboutErrorAction } from "../../store/error-notificator/error-notificator-actions";

export enum HttpMethod {
  GET = "get",
  POST = "post",
}

export interface ContainsTraceId {
  traceId: string;
}

export type EmptyResponse = ContainsTraceId;

export interface Success<T> extends EmptyResponse {
  data: T;
}

export interface ErrorResponse extends EmptyResponse {
  type: string;
  description: string;
}

export interface RequestMeta {
  url: string;
  method: HttpMethod;
}

export interface ApiRequest<T> extends RequestMeta {
  query?: Record<string, string> | null;
  body?: T | null;
}

const baseUrl = `${window.location.protocol}//${window.location.host}`;

function call<R>(request: ApiRequest<any>): Promise<R> {
  const url = new URL(request.url, baseUrl);
  if (request.query) {
    url.search = new URLSearchParams(request.query).toString();
  }
  return fetch(request.url, {
    method: request.method,
    headers: {
      "Content-Type": "application/json",
    },
    body: request.body ? JSON.stringify(request.body) : null,
  }).then(async (response) => {
    const json = await response.json();
    if (!response.ok) {
      store.dispatch(NotifyAboutErrorAction(json as ErrorResponse));
    }
    return json as R;
  });
}

export function queryCall<T, R>(request: ApiRequest<T>): Promise<R> {
  return call<Success<R>>(request).then((x) => x.data);
}

export async function commandCall<T>(request: ApiRequest<T>): Promise<void> {
  await call<EmptyResponse>(request);
}
