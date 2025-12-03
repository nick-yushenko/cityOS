import { create } from "zustand";
import { Budget, BudgetRevision, IncomeExecution, IncomePlan, IncomeSource } from "./types";

interface BudgetState {
  budgetsById: Record<number, Budget>;
  budgetRevisionsById: Record<number, BudgetRevision>;
  incomePlansById: Record<number, IncomePlan>;
  incomeExecutionsById: Record<number, IncomeExecution>;
  incomeSourcesById: Record<number, IncomeSource>;

  // actions
  setBudgets: (budgets: Budget[]) => void;
  setBudgetRevisions: (budgetRevisions: BudgetRevision[]) => void;
  setIncomePlans: (incomePlans: IncomePlan[]) => void;
  setIncomeExecutions: (incomeExecutions: IncomeExecution[]) => void;
  setIncomeSources: (incomeSources: IncomeSource[]) => void;
}

const useBudgetStore = create<BudgetState>((set) => ({
  budgetsById: {},
  budgetRevisionsById: {},
  incomePlansById: {},
  incomeExecutionsById: {},
  incomeSourcesById: {},

  setBudgets: (budgets: Budget[]) =>
    set((state) => ({
      budgetsById: {
        ...state.budgetsById,
        ...Object.fromEntries(budgets.map((budget) => [budget.id, budget])),
      },
    })),
  setBudgetRevisions: (budgetRevisions: BudgetRevision[]) =>
    set((state) => ({
      budgetRevisionsById: {
        ...state.budgetRevisionsById,
        ...Object.fromEntries(
          budgetRevisions.map((budgetRevision) => [budgetRevision.id, budgetRevision])
        ),
      },
    })),
  setIncomePlans: (incomePlans: IncomePlan[]) =>
    set((state) => ({
      incomePlansById: {
        ...state.incomePlansById,
        ...Object.fromEntries(incomePlans.map((incomePlan) => [incomePlan.id, incomePlan])),
      },
    })),
  setIncomeExecutions: (incomeExecutions: IncomeExecution[]) =>
    set((state) => ({
      incomeExecutionsById: {
        ...state.incomeExecutionsById,
        ...Object.fromEntries(
          incomeExecutions.map((incomeExecution) => [incomeExecution.id, incomeExecution])
        ),
      },
    })),
  setIncomeSources: (incomeSources: IncomeSource[]) =>
    set((state) => ({
      incomeSourcesById: {
        ...state.incomeSourcesById,
        ...Object.fromEntries(incomeSources.map((incomeSource) => [incomeSource.id, incomeSource])),
      },
    })),
}));
