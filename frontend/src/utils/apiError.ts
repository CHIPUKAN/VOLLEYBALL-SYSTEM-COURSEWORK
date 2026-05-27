// извлекает человекочитаемое сообщение из ошибки axios или Error
export function getApiError(err: unknown, fallback: string): string {
  const axErr = err as { response?: { data?: { message?: string; title?: string } } };
  return (
    axErr?.response?.data?.message ??
    axErr?.response?.data?.title ??
    (err instanceof Error ? err.message : undefined) ??
    fallback
  );
}
