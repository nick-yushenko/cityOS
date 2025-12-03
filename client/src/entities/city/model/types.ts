import { Budget } from "@/entities/budget/model/types";
import { DataSource } from "@/entities/data-source/model/types";

export interface City {
  id: number;
  name: string;
  code: string;
  dataSources: DataSource[];
  budgets: Budget[];
}
