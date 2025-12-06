"use client";

import { create } from "zustand";
import { getCities } from "./api";
import { City } from "./types";

interface CityState {
  citiesById: Record<number, City>;

  isLoaded: boolean;
  loadCities: () => Promise<void>;
}

export const useCityStore = create<CityState>()((set, get) => ({
  citiesById: {},
  isLoaded: false,
  loadCities: async (options: { force?: boolean } = { force: false }) => {
    const { force } = options;
    if (!force && get().isLoaded) {
      return;
    }
    const cities = await getCities();

    set({
      citiesById: Object.fromEntries(cities.map((city) => [city.id, city])),
      isLoaded: true,
    });
  },
}));
