"use client";

import { Button } from "@/shared/ui/button";
import { CheckboxField } from "@/shared/ui/checkbox";
import { InputField } from "@/shared/ui/input-field";
import { SelectField } from "@/shared/ui/select-field";
import { AddDataSourceFormValues, addDataSourceSchema } from "../lib/validation";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useDataSourceStore } from "@/entities/data-source/model/store";
import { Alert, Box } from "@mui/material";
import { useState } from "react";
import { useCitiesView } from "@/entities/city/model/selectors";
import { CitySelect } from "@/entities/city/ui/city-select";

export const AddDataSourceForm: React.FC = () => {
  const { addSource, loading, error } = useDataSourceStore();
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const { reset, handleSubmit, control } = useForm<AddDataSourceFormValues>({
    resolver: zodResolver(addDataSourceSchema),
    defaultValues: {
      name: "",
      sourceUrl: "",
      description: "",
      cityId: "",
      datasetKind: "",
      fileType: "",
      isActive: true,
    },
  });

  const onSubmit = async (values: AddDataSourceFormValues) => {
    setSuccessMessage(null);
    await addSource({
      name: values.name,
      cityId: Number(values.cityId),
      description: values.description,
      sourceUrl: values.sourceUrl,
      datasetKind: values.datasetKind,
      fileType: "",
      isActive: true,
    });

    // Получаем актуальное значение error из store, так как компонент еще не перерендерился
    const currentError = useDataSourceStore.getState().error;
    if (!currentError) {
      reset();
      setSuccessMessage("Источник данных успешно добавлен");
    }
  };
  return (
    <Box
      component="form"
      onSubmit={handleSubmit(onSubmit)}
      sx={{ mb: 4, display: "flex", flexDirection: "column", gap: 2, width: "100%" }}
    >
      <InputField name="name" control={control} label="Название" />
      <InputField name="description" control={control} label="Описание" />

      <CitySelect name="cityId" control={control} label="Город" />

      <InputField name="datasetKind" control={control} label="Тип данных" />
      <InputField name="sourceUrl" control={control} label="URL" />
      <InputField name="fileType" control={control} label="Тип файла" />

      <CheckboxField name="isActive" control={control} label="Активный источник" />

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
          {loading ? "Сохраняем…" : "Добавить источник"}
        </Button>
      </Box>
    </Box>
  );
};
