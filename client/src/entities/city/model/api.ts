"use client";

import { apiFetch } from "@/shared/api/client";
import { City } from "./types";

export const getCities = async (): Promise<City[]> => {
  return apiFetch<City[]>("/cities");
};
