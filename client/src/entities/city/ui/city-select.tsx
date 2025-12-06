"use client";

import { SelectField } from "@/shared/ui/select-field";
import { Control, FieldValues } from "react-hook-form";
import { useCitiesView } from "../model/selectors";
import { useCityStore } from "../model/store";
import { useEffect, useMemo } from "react";

type CitySelectProps = {
  name: string;
  control: Control<any>;
  label: string;
};
export const CitySelect = ({ name = "cityId", control, label = "Город" }: CitySelectProps) => {
  const loadCities = useCityStore((s) => s.loadCities);
  const cities = useCitiesView();

  useEffect(() => {
    loadCities();
  }, [loadCities]);

  return (
    <SelectField
      name={name}
      control={control}
      label={label}
      options={cities.map((city) => ({ value: String(city.id), label: city.name }))}
    />
  );
};
