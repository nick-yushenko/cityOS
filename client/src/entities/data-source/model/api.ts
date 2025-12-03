"use client";

import { apiFetch } from "@/shared/api/client";
import { DataSource, DataSourcePayload } from "./types";

export const getSources = async (): Promise<DataSource[]> => {
  return apiFetch<DataSource[]>("/data-sources");
};

export const getSource = async (id: number): Promise<DataSource> => {
  return apiFetch<DataSource>(`/data-sources/${id}`);
};

export const updateSource = async (id: number, source: DataSourcePayload): Promise<DataSource> => {
  return apiFetch<DataSource>(`/data-sources/${id}`, {
    method: "PATCH",
    body: source,
  });
};

export const deleteSource = async (id: number): Promise<void> => {
  return apiFetch<void>(`/data-sources/${id}`, {
    method: "DELETE",
  });
};

export const addSource = async (source: DataSourcePayload): Promise<DataSource> => {
  return apiFetch<DataSource>("/data-sources", {
    method: "POST",
    body: source,
  });
};
