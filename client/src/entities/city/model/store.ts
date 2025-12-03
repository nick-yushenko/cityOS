"use client";

import { create } from "zustand";
import { getCities } from "./api";
import { City } from "./types";

interface CityState {
  citiesById: Record<number, City>;

  loadCities: () => Promise<void>;
}

export const useCityStore = create<CityState>()((set) => ({
  citiesById: {},
  loadCities: async () => {
    const cities = await getCities();

    set({
      citiesById: Object.fromEntries(cities.map((city) => [city.id, city])),
    });
  },
}));
