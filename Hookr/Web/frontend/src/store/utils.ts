export interface Action<T, P> {
  type: T;
  props: P;
}
