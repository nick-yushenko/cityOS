"use client";

import { create } from "zustand";
import { DataSource, DataSourcePayload } from "./types";
import { addSource, deleteSource, getSources, updateSource } from "./api";
import { parseApiError } from "@/shared/api/error-parser";

interface DataSourceState {
  sourcesById: Record<number, DataSource>;

  loading: boolean;
  error: string | null;

  loadDataSources: () => Promise<void>;
  addSource: (source: DataSourcePayload) => Promise<void>;
  updateSource: (id: number, source: DataSourcePayload) => Promise<void>;
  deleteSource: (id: number) => Promise<void>;
}

export const useDataSourceStore = create<DataSourceState>()((set) => ({
  sourcesById: {},

  loading: false,
  error: null,

  loadDataSources: async () => {
    set({ loading: true, error: null });

    const sources: DataSource[] = await getSources();
    set({
      sourcesById: Object.fromEntries(sources.map((s) => [s.id, s])),
      loading: false,
      error: null,
    });
  },
  addSource: async (source: DataSourcePayload) => {
    set({ loading: true, error: null });
    try {
      const newSource: DataSource = await addSource(source);
      set((state) => ({
        sourcesById: { ...state.sourcesById, [newSource.id]: newSource },
        loading: false,
        error: null,
      }));
    } catch (err) {
      const errorMessage = parseApiError(err);
      set({ loading: false, error: errorMessage });
    }
  },

  updateSource: async (id: number, source: DataSourcePayload) => {
    const updatedSource: DataSource = await updateSource(id, source);
    set((state) => ({
      sourcesById: { ...state.sourcesById, [id]: updatedSource },
    }));
  },
  deleteSource: async (id: number) => {
    await deleteSource(id);
    set((state) => {
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      const { [id]: _, ...rest } = state.sourcesById;
      return { sourcesById: { ...rest } };
    });
  },
}));
