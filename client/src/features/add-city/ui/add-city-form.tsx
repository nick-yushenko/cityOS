"use client";

import { Button } from "@/shared/ui/button";
import { InputField } from "@/shared/ui/input-field";
import { AddCityFormValues, addCitySchema } from "../lib/validation";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Alert, Box } from "@mui/material";
import { useState } from "react";
import { useCityStore } from "@/entities/city/model/store";

export const AddCityForm: React.FC = () => {
  const { addCity, loading, error } = useCityStore();
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const { reset, handleSubmit, control } = useForm<AddCityFormValues>({
    resolver: zodResolver(addCitySchema),
    defaultValues: {
      name: "",
      code: "",
    },
  });

  const onSubmit = async (values: AddCityFormValues) => {
    setSuccessMessage(null);
    await addCity({
      name: values.name,
      code: values.code,
    });

    // Получаем актуальное значение error из store, так как компонент еще не перерендерился
    const currentError = useCityStore.getState().error;
    if (!currentError) {
      reset();
      setSuccessMessage("Город успешно добавлен");
    }
  };
  return (
    <Box
      component="form"
      onSubmit={handleSubmit(onSubmit)}
      sx={{ display: "flex", flexDirection: "column", gap: 2 }}
    >
      <InputField name="name" control={control} label="Название" />

      <InputField name="code" control={control} label="Код" />

      {error && (
        <Alert severity="error" variant="outlined" sx={{ whiteSpace: "pre-line" }}>
          {error}
        </Alert>
      )}

      {successMessage && (
        <Alert severity="success" variant="outlined">
          {successMessage}
        </Alert>
      )}

      <Box sx={{ display: "flex", justifyContent: "flex-end" }}>
        <Button type="submit" variant="contained" disabled={loading}>
          {loading ? "Сохраняем…" : "Добавить"}
        </Button>
      </Box>
    </Box>
  );
};
