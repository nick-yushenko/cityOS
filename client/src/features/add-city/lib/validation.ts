"use client";

import { z } from "zod";

export const addCitySchema = z.object({
  name: z.string().min(3, "Минимум 3 символа").max(255, "Максимум 255 символов"),
  code: z.string().min(3, "Код состоит минимум из 3 символов").max(50, "Максимум 50 символов"),
});

export const validateAddCity = (data: unknown) => {
  return addCitySchema.safeParse(data);
};

export type AddCityFormValues = z.infer<typeof addCitySchema>;
