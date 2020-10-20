export function toDataCheckString(data: object): string {
  return Object.entries(data)
    .map((x) => `${x[0]}=${x[1]}`)
    .sort()
    .join("\n");
}
