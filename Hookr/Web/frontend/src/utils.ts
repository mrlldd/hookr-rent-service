export function cut<T>(data: T, ...keys: (keyof T)[]): object {
  return Object.fromEntries(
    Object.entries(data).filter((x) => !keys.includes(x[0] as keyof T))
  );
}

export function toDataCheckString(data: object): string {
  return Object.entries(data)
    .map((x) => `${x[0]}=${x[1]}`)
    .sort()
    .join("\n");
}

export interface KeyValuePair<K, V> {
  key: K;
  value: V;
}
