"use client";

import { useCityStore } from "./store";
import { City } from "./types";

export const useCityView = (cityId: number): City | null => {
  return useCityStore((state) => state.citiesById[cityId] ?? null);
};

export const useCitiesView = (): City[] => {
  const citiesById = useCityStore((s) => s.citiesById);
  return Object.values(citiesById);
};

// export const useCityView = (cityId: number): City | null => {
//   return useCityStore((state) => state.cities.find((city) => city.id === cityId) ?? null);
// };

// export const useCitiesView = (): City[] => {
//   return useCityStore((state) => state.cities);
// };
