"use client";

import { useCitiesView } from "@/entities/city/model/selectors";
import { useCityStore } from "@/entities/city/model/store";
import { AddCityForm } from "@/features/add-city/ui/add-city-form";
import { DataGridComponent } from "@/shared/ui/data-grid";
import { GridColDef } from "@mui/x-data-grid";

export const CityTable = () => {
  const { loadCities, loading } = useCityStore();

  const cities = useCitiesView();

  const columns: GridColDef[] = [
    { field: "id", headerName: "ID", width: 100 },
    { field: "name", headerName: "Название", width: 200 },
    { field: "code", headerName: "Код", width: 300 },
  ];

  return (
    <DataGridComponent
      rows={cities}
      columns={columns}
      loadData={loadCities}
      loading={loading}
      addEntityComponent={<AddCityForm />}
    />
  );
};
