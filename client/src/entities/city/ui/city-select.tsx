"use client";

import { SelectField } from "@/shared/ui/select-field";
import { Control } from "react-hook-form";
import { useCitiesView } from "@/entities/city/model/selectors";
import { useCityStore } from "@/entities/city/model/store";
import { useEffect } from "react";

type CitySelectProps = {
  name: string;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
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
