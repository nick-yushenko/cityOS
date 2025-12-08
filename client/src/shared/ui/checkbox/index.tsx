"use client";

import { Checkbox, CheckboxProps, FormControlLabel, FormControlLabelProps } from "@mui/material";
import { Control, Controller, FieldPath, FieldValues } from "react-hook-form";

type CheckboxFieldProps<T extends FieldValues> = {
  name: FieldPath<T>;
  control: Control<T>;
  label: string;
  checkboxProps?: Omit<CheckboxProps, "checked" | "onChange">;
} & Omit<FormControlLabelProps, "control" | "label">;

export const CheckboxField = <T extends FieldValues>({
  name,
  control,
  label,
  checkboxProps,
  ...formControlLabelProps
}: CheckboxFieldProps<T>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field }) => (
        <FormControlLabel
          label={label}
          control={
            <Checkbox
              checked={field.value ?? false}
              onChange={(e, checked) => field.onChange(checked)}
              {...checkboxProps}
            />
          }
          {...formControlLabelProps}
        />
      )}
    />
  );
};
