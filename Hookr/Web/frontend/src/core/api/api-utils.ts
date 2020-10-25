import { getFromLocalStorage } from "../../context/local-storage-utils";

export enum HttpMethod {
  GET = "get",
  POST = "post",
  DELETE = "delete",
}

interface ContainsTraceId {
  traceId?: string;
}

export interface HasSuccess {
  success: boolean; // inbound property, not present in real responses
}

export type EmptyResponse = ContainsTraceId & HasSuccess;

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

function call<R>(
  request: ApiRequest<any>
): Promise<Success<R> | ErrorResponse> {
  const url = new URL(request.url, baseUrl);
  if (request.query) {
    url.search = new URLSearchParams(request.query).toString();
  }
  const headers: HeadersInit = {
    "Content-Type": "application/json",
  };
  const token = getFromLocalStorage("token");
  if (token) {
    headers["Authorization"] = "Bearer " + token;
  }
  return fetch(request.url, {
    method: request.method,
    headers: headers,
    body: request.body && JSON.stringify(request.body),
  }).then((x) => readResponseAsync<R>(x));
}

async function readResponseAsync<R>(
  response: Response
): Promise<Success<R> | ErrorResponse> {
  if (response.ok) {
    return await readBodyAs<Success<R>>(response);
  }
  try {
    return await readBodyAs<ErrorResponse>(response);
  } catch (e) {
    return parseResponseToError(response);
  }
}

async function readBodyAs<R extends HasSuccess>(
  response: Response
): Promise<R> {
  const json = await response.json();
  const result = json as R;
  result.success = response.ok;
  return result;
}

function parseResponseToError(response: Response): ErrorResponse {
  return {
    success: false,
    type: `UnknownError`,
    description: `${response.url} returned response with status: ${response.status}`,
    traceId: undefined,
  };
}

export function queryCall<T, R>(
  request: ApiRequest<T>
): Promise<Success<R> | ErrorResponse> {
  return call(request);
}

export function commandCall<T>(
  request: ApiRequest<T>
): Promise<EmptyResponse | ErrorResponse> {
  return call(request);
}
