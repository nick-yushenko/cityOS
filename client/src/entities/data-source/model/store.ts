"use client";

import { create } from "zustand";
import { DataSource, DataSourcePayload } from "./types";
import { addSource, deleteSource, getSources, updateSource } from "./api";

interface DataSourceState {
  sourcesById: Record<number, DataSource>;

  loading: boolean;
  error: string | null;

  loadSources: () => Promise<void>;
  addSource: (source: DataSourcePayload) => Promise<void>;
  updateSource: (id: number, source: DataSourcePayload) => Promise<void>;
  deleteSource: (id: number) => Promise<void>;
}

export const useDataSourceStore = create<DataSourceState>()((set) => ({
  sourcesById: {},

  loading: false,
  error: null,

  loadSources: async () => {
    const sources: DataSource[] = await getSources();
    set({
      sourcesById: Object.fromEntries(sources.map((s) => [s.id, s])),
    });
  },
  addSource: async (source: DataSourcePayload) => {
    const newSource: DataSource = await addSource(source);
    set((state) => ({
      sourcesById: { ...state.sourcesById, [newSource.id]: newSource },
    }));
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
