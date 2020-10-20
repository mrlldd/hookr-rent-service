export enum HttpMethod {
  GET = "get",
  POST = "post",
}

interface ContainsTraceId {
  traceId: string;
}

export interface Response {
  success: boolean; // inbound property, not present in real responses
}

export type EmptyResponse = ContainsTraceId & Response;

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

function call<R extends EmptyResponse>(
  request: ApiRequest<any>
): Promise<R | ErrorResponse> {
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
    const result = json as R | ErrorResponse;
    result.success = response.ok;
    return result;
  });
}

export function queryCall<T, R>(
  request: ApiRequest<T>
): Promise<Success<R> | ErrorResponse> {
  return call<Success<R>>(request);
}

export function commandCall<T>(
  request: ApiRequest<T>
): Promise<EmptyResponse | ErrorResponse> {
  return call(request);
}
