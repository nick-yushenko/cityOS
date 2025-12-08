"use client";

import { Box, ClickAwayListener, Paper, Popper, Tooltip, Typography } from "@mui/material";
import { DataGrid, DataGridProps, GridColDef, Toolbar, ToolbarButton } from "@mui/x-data-grid";
import { useEffect, useState } from "react";
import RefreshIcon from "@mui/icons-material/Refresh";
import AddIcon from "@mui/icons-material/Add";
import React from "react";

interface DataGridComponentProps<T = unknown> extends Omit<DataGridProps, "rows" | "columns"> {
  rows: readonly T[];
  columns: GridColDef[];

  loadData?: () => void;
  loading: boolean;

  addEntityComponent?: React.ReactNode;
}

interface CustomToolbarProps {
  loadData: () => void;
  loading: boolean;
  addEntityComponent?: React.ReactNode;
}

const CustomToolbar = ({ loadData, loading, addEntityComponent }: CustomToolbarProps) => {
  const [anchorEl, setAnchorEl] = useState<HTMLButtonElement | null>(null);
  const [newPanelOpen, setNewPanelOpen] = useState(false);

  const handleClose = () => {
    setNewPanelOpen(false);
    setAnchorEl(null);
  };

  const handleKeyDown = (event: React.KeyboardEvent) => {
    if (event.key === "Escape") {
      handleClose();
    }
  };

  const handleToggle = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
    setNewPanelOpen((prev) => !prev);
  };

  return (
    <Toolbar>
      {addEntityComponent && (
        <>
          <Tooltip title="Добавить">
            <ToolbarButton aria-describedby="new-panel" onClick={handleToggle}>
              <AddIcon fontSize="small" />
            </ToolbarButton>
          </Tooltip>
          <Popper
            open={newPanelOpen}
            anchorEl={anchorEl}
            placement="bottom-end"
            id="new-panel"
            onKeyDown={handleKeyDown}
          >
            <ClickAwayListener onClickAway={handleClose}>
              <Paper
                sx={{
                  display: "flex",
                  flexDirection: "column",
                  gap: 2,
                  width: 400,
                  p: 2,
                }}
                elevation={8}
              >
                <Typography fontWeight="bold">Добавить</Typography>

                {addEntityComponent}
              </Paper>
            </ClickAwayListener>
          </Popper>
        </>
      )}

      <ToolbarButton onClick={loadData} disabled={loading} title="Обновить данные">
        <RefreshIcon fontSize="small" />
      </ToolbarButton>
    </Toolbar>
  );
};

export const DataGridComponent = <T = unknown,>({
  rows,
  columns,
  loadData,
  loading,
  addEntityComponent,
  ...rest
}: DataGridComponentProps<T>) => {
  useEffect(() => {
    if (loadData) {
      loadData();
    }
  }, [loadData]);

  return (
    <Box sx={{ width: "100%", marginBottom: 4 }}>
      <DataGrid
        rows={rows}
        columns={columns}
        slots={{
          toolbar: () => (
            <CustomToolbar
              loadData={loadData ?? (() => {})}
              loading={loading}
              addEntityComponent={addEntityComponent}
            />
          ),
        }}
        showToolbar
        loading={loading}
        {...rest}
      />
    </Box>
  );
};
