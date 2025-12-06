/**
 * Создает дедуплицированную версию асинхронной функции.
 * Если функция вызывается с одинаковыми аргументами несколько раз до завершения первого вызова,
 * все вызовы получат один и тот же Promise, и реальная функция выполнится только один раз.
 */
export const createDeduplicated = <Args extends any[], R>(fn: (...args: Args) => Promise<R>) => {
  const cache: Record<string, Promise<R>> = {};

  return (...args: Args) => {
    const key = JSON.stringify([...args]);
    if (!(key in cache)) {
      cache[key] = fn(...args).finally(() => {
        delete cache[key];
      });
    }
    return cache[key];
  };
};
