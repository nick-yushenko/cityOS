"use client";

import { DataSource } from "./types";
import { useDataSourceStore } from "./store";

export const useDataSourceView = (dataSourceId: number): DataSource | null => {
  return useDataSourceStore((state) => state.sourcesById[dataSourceId] ?? null);
};
export const useDataSourcesView = (): DataSource[] => {
  const sourcesById = useDataSourceStore((s) => s.sourcesById);
  return Object.values(sourcesById);
};
