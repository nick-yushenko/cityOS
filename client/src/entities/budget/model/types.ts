export interface Budget {
  id: number;
  cityId: number;
  revisionIds: number[];
}

export interface BudgetRevision {
  id: number;
  year: number;
  name: string;
  revisionDate: Date;
  cityId: number;
  incomePlanIds: number[];
  incomeExecutionIds: number[];
}

// Источник дохода для бюджета. Например: НДФЛ, акцизы и тд и тп.
export interface IncomeSource {
  id: number;
  name: string;
  externalCode: string;
  description: string;
}

export interface IncomePlan {
  id: number;
  revisionId: number;
  incomeSourceId: number;
  value: number;
  sharePercent: number;
  createdAt: Date;
  updatedAt: Date;
}

export interface IncomeExecution {
  id: number;
  revisionId: number;
  incomeSourceId: number;
  reportPeriod: Date;
  value: number;
  executionPercent: number;
  createdAt: Date;
  updatedAt: Date;
}
