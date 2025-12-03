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

export async function apiFetch<T>(path: string, options: ApiOptions = {}): Promise<T> {
  const { method = "GET", params, body } = options;

  const query = buildQuery(params);
  const url = `${API_BASE_URL}${path}${query}`;

  const res = await fetch(url, {
    method,
    headers: {
      "Content-Type": "application/json",
    },
    body: method === "GET" ? undefined : JSON.stringify(body),
  });

  if (!res.ok) {
    const text = await res.text().catch(() => "");
    console.error(`API error ${res.status}: ${text || res.statusText}`);
    throw new Error(`API error ${res.status}: ${text || res.statusText}`);
  }

  return (await res.json()) as T;
}
