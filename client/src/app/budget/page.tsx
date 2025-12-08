"use client";

import { Container, Typography, Box } from "@mui/material";
import { CityTable } from "@/entities/city/ui/city-table";
import { DataSourceTable } from "@/entities/data-source/ui/data-source-table";

export default function Page() {
  return (
    <Container maxWidth="xl">
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom fontWeight={600}>
          Бюджет города
        </Typography>

        <CityTable />
        <DataSourceTable />
      </Box>
    </Container>
  );
}
