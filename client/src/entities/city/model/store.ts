"use client";

import { create } from "zustand";
import { addCity, getCities } from "./api";
import { City, CityPayload } from "./types";
import { parseApiError } from "@/shared/api/error-parser";

interface CityState {
  citiesById: Record<number, City>;

  loading: boolean;
  error: string | null;

  loadCities: () => Promise<void>;
  addCity: (city: CityPayload) => Promise<void>;
}

export const useCityStore = create<CityState>()((set) => ({
  citiesById: {},
  loading: false,
  error: null,

  loadCities: async () => {
    set({ loading: true });
    const cities = await getCities();

    set({
      citiesById: Object.fromEntries(cities.map((city) => [city.id, city])),
      loading: false,
    });
  },

  addCity: async (city: CityPayload) => {
    set({ loading: true });
    try {
      const newCity: City = await addCity(city);
      set((state) => ({
        citiesById: { ...state.citiesById, [newCity.id]: newCity },
        loading: false,
      }));
    } catch (error) {
      const errorMessage = parseApiError(error);
      set({ loading: false, error: errorMessage });
    }
  },
}));
