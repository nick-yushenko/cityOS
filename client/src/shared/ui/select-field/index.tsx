"use client";

import {
  FormControl,
  FormControlProps,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
  SelectProps,
} from "@mui/material";
import { Control, Controller, FieldPath, FieldValues } from "react-hook-form";

type SelectOption = {
  value: string | number;
  label: string;
};

type SelectFieldProps<T extends FieldValues> = {
  name: FieldPath<T>;
  control: Control<T>;
  label: string;
  options: SelectOption[];
  formControlProps?: Omit<FormControlProps, "error" | "fullWidth">;
  errorText?: string;
} & Omit<SelectProps, "value" | "onChange" | "label" | "labelId">;

export const SelectField = <T extends FieldValues>({
  name,
  control,
  label,
  options,
  formControlProps,
  ...selectProps
}: SelectFieldProps<T>) => {
  const labelId = `${name}-select-label`;

  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { error } }) => (
        <FormControl fullWidth error={!!error} {...formControlProps}>
          <InputLabel id={labelId}>{label}</InputLabel>
          <Select
            labelId={labelId}
            label={label}
            value={field.value ?? ""}
            onChange={field.onChange}
            onBlur={field.onBlur}
            {...selectProps}
          >
            {options.map((option) => (
              <MenuItem key={option.value} value={option.value}>
                {option.label}
              </MenuItem>
            ))}
          </Select>
          {error && <FormHelperText>{error.message}</FormHelperText>}
        </FormControl>
      )}
    />
  );
};
