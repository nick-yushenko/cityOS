"use client";

import { useDataSourcesView } from "@/entities/data-source/model/selectors";
import { useDataSourceStore } from "@/entities/data-source/model/store";
import { AddDataSourceForm } from "@/features/add-data-source/ui/add-data-source-form";
import { DataGridComponent } from "@/shared/ui/data-grid";
import { GridColDef } from "@mui/x-data-grid";

export const DataSourceTable = () => {
  const { loadDataSources, loading } = useDataSourceStore();

  const dataSources = useDataSourcesView();

  const columns: GridColDef[] = [
    { field: "id", headerName: "ID", width: 100 },
    { field: "name", headerName: "Название", width: 200 },
    { field: "datasetKind", headerName: "Тип набора данных", width: 300 },
    { field: "sourceUrl", headerName: "URL источника", width: 400 },
    { field: "fileType", headerName: "Тип файла", width: 200 },
    { field: "description", headerName: "Описание", width: 300 },
    { field: "isActive", headerName: "Активен", width: 100 },
    { field: "lastLoadedAt", headerName: "Последнее обновление", width: 200 },
    { field: "lastChecksum", headerName: "Хэш", width: 200 },
  ];

  return (
    <DataGridComponent
      rows={dataSources}
      columns={columns}
      loadData={loadDataSources}
      loading={loading}
      addEntityComponent={<AddDataSourceForm />}
    />
  );
};
