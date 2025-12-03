export interface DataSource {
  id: number;
  cityId: number;
  name: string;
  datasetKind: string;
  sourceUrl: string;
  fileType: string;
  description?: string;
  isActive: boolean;
  lastLoadedAt: Date;
}

export type DataSourcePayload = Omit<DataSource, "id" | "lastLoadedAt">;
