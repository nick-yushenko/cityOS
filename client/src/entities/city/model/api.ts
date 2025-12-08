"use client";

import { apiFetch } from "@/shared/api/client";
import { City, CityPayload } from "./types";

export const getCities = async (): Promise<City[]> => {
  return apiFetch<City[]>("/cities");
};

export const addCity = async (city: CityPayload): Promise<City> => {
  return apiFetch<City>("/cities", {
    method: "POST",
    body: city,
  });
};
