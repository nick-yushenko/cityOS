interface ApiErrorResponse {
  errors?: Record<string, string[]>;
  title?: string;
  message?: string;
}

/**
 * Парсит ошибку API и возвращает читаемое сообщение
 */
export function parseApiError(error: unknown): string {
  if (!(error instanceof Error)) {
    return "Произошла неизвестная ошибка";
  }

  const message = error.message;

  // Пытаемся найти JSON в сообщении об ошибке
  // Формат: "API error 400: {...json...}"
  const jsonMatch = message.match(/\{[\s\S]*\}/);
  if (jsonMatch) {
    try {
      const errorData: ApiErrorResponse = JSON.parse(jsonMatch[0]);

      // Если есть errors (валидационные ошибки)
      if (errorData.errors && Object.keys(errorData.errors).length > 0) {
        const errorMessages: string[] = [];
        Object.entries(errorData.errors).forEach(([field, messages]) => {
          messages.forEach((msg) => {
            errorMessages.push(`- ${msg}`);
          });
        });
        return `Возникла ошибка:\n${errorMessages.join("\n")}`;
      }

      // Если есть title или message
      if (errorData.title) {
        return errorData.title;
      }
      if (errorData.message) {
        return errorData.message;
      }
    } catch {
      // Если не удалось распарсить JSON, продолжаем с исходным сообщением
    }
  }

  // Если не удалось распарсить, возвращаем исходное сообщение
  // Убираем префикс "API error XXX: " для более чистого отображения
  return message.replace(/^API error \d+: /, "");
}
