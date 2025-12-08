import { createDeduplicated } from "../lib/create-deduplicated";
import { API_BASE_URL } from "./config";
import { ApiOptions } from "./types";

function buildQuery(params?: ApiOptions["params"]): string {
  if (!params) return "";
  const sp = new URLSearchParams();

  Object.entries(params).forEach(([key, value]) => {
    if (value === undefined) return;
    sp.append(key, String(value));
  });

  const qs = sp.toString();
  return qs ? `?${qs}` : "";
}

const deduplicatedFetch = createDeduplicated(async (url: string, options: RequestInit) => {
  return fetch(url, options);
});

export const apiFetch = async <T>(path: string, options: ApiOptions = {}): Promise<T> => {
  const { method = "GET", params, body, force = false } = options;

  const baseOptions: RequestInit = {
    method,
    headers: {
      "Content-Type": "application/json",
    },
  };
  const query = buildQuery(params);
  const url = `${API_BASE_URL}${path}${query}`;

  let res: Response;
  if (method === "GET" && !force) {
    res = await deduplicatedFetch(url, baseOptions);
    // Клонируем ответ чтобы избежать проблем с замыканием Promise<Response>
    res = await res.clone();
  } else {
    res = await fetch(url, {
      ...baseOptions,
      body: method === "GET" ? undefined : JSON.stringify(body),
    });
  }

  if (!res.ok) {
    const text = await res.text().catch(() => "");
    console.error(`API error ${res.status}: ${text || res.statusText}`);
    throw new Error(`API error ${res.status}: ${text || res.statusText}`);
  }

  return (await res.json()) as T;
};
