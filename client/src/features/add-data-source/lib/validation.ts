"use client";

import { z } from "zod";

export const addDataSourceSchema = z.object({
  name: z.string().min(3, "Минимум 3 символа").max(255, "Максимум 255 символов"),
  description: z.string().max(1000, "Максимум 1000 символов").optional(),
  cityId: z.string().min(1, "Город обязателен"),
  //   cityId: z.coerce.number<number>().min(1, "Город обязателен"),
  datasetKind: z.string().min(1, "Тип данных обязателен"),
  sourceUrl: z.string().url("Неверный URL"),
  fileType: z.string().min(1, "Тип файла обязателен"),
  isActive: z.boolean(),
});

export const validateAddDataSource = (data: unknown) => {
  return addDataSourceSchema.safeParse(data);
};

export type AddDataSourceFormValues = z.infer<typeof addDataSourceSchema>;
