"use client";

import { TextField, TextFieldProps } from "@mui/material";
import { Control, Controller, FieldPath, FieldValues } from "react-hook-form";

type InputFieldProps<T extends FieldValues> = {
  name: FieldPath<T>;
  control: Control<T>;
  label: string;
} & Omit<TextFieldProps, "name" | "value" | "onChange" | "onBlur" | "ref">;

export const InputField = <T extends FieldValues>({
  name,
  control,
  label,
  ...textFieldProps
}: InputFieldProps<T>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { error } }) => (
        <TextField
          label={label}
          {...field}
          error={!!error}
          helperText={error?.message}
          {...textFieldProps}
        />
      )}
    />
  );
};
