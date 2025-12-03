"use client";

import { Button } from "@/shared/ui/button";
import { AddDataSourceFormValues, addDataSourceSchema } from "../lib/validation";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useDataSourceStore } from "@/entities/data-source/model/store";
import {
  Alert,
  Box,
  Checkbox,
  FormControl,
  FormControlLabel,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
  TextField,
} from "@mui/material";
import { useState } from "react";
import { useCitiesView } from "@/entities/city/model/selectors";

export const AddDataSourceForm: React.FC = () => {
  const cities = useCitiesView();

  const { addSource, loading, error } = useDataSourceStore();
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const {
    register,
    reset,
    handleSubmit,
    control,
    formState: { errors },
  } = useForm<AddDataSourceFormValues>({
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
      fileType: values.fileType,
      isActive: true,
    });

    if (!error) {
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
      <TextField
        label="Название"
        {...register("name")}
        error={!!errors.name}
        helperText={errors.name?.message}
      />
      <TextField
        label="Описание"
        {...register("description")}
        error={!!errors.description}
        helperText={errors.description?.message}
      />

      <Controller
        name="cityId"
        control={control}
        render={({ field }) => (
          <FormControl fullWidth error={!!errors.cityId}>
            <InputLabel id="city-select-label">Город</InputLabel>

            <Select
              labelId="city-select-label"
              label="Город"
              value={field.value}
              onChange={field.onChange}
            >
              {cities.map((city) => (
                <MenuItem key={city.id} value={city.id}>
                  {city.name}
                </MenuItem>
              ))}
            </Select>

            <FormHelperText>{errors.cityId?.message}</FormHelperText>
          </FormControl>
        )}
      />

      <TextField
        label="Тип данных"
        {...register("datasetKind")}
        error={!!errors.datasetKind}
        helperText={errors.datasetKind?.message}
      />
      <TextField
        label="URL"
        {...register("sourceUrl")}
        error={!!errors.sourceUrl}
        helperText={errors.sourceUrl?.message}
      />
      <TextField
        label="Тип файла"
        {...register("fileType")}
        error={!!errors.fileType}
        helperText={errors.fileType?.message}
      />

      <Controller
        name="isActive"
        control={control}
        render={({ field }) => (
          <FormControlLabel
            label="Активный источник"
            control={
              <Checkbox checked={field.value} onChange={(e, checked) => field.onChange(checked)} />
            }
          />
        )}
      />

      {error && (
        <Alert severity="error" variant="outlined">
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
