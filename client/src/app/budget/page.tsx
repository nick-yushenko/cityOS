"use client";

import { Container, Typography, Box } from "@mui/material";
import { useCityStore } from "@/entities/city/model/store";
import { useEffect } from "react";
import { useCitiesView } from "@/entities/city/model/selectors";
import { AddDataSourceForm } from "@/features/add-data-source/ui/addDataSourceForm";

export default function Page() {
  const loadCities = useCityStore((s) => s.loadCities);
  const cities = useCitiesView();

  useEffect(() => {
    loadCities();
  }, [loadCities]);

  return (
    <Container maxWidth="xl">
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom fontWeight={600}>
          Бюджет города
        </Typography>

        <h1 style={{ marginBottom: 16 }}>Источники данных</h1>

        <AddDataSourceForm />
        {cities.map((city) => (
          <div key={city.id}>
            <h2>{city.name}</h2>
            <ul>
              {city.dataSources.map((ds) => (
                <li key={ds.id}>{ds.datasetKind}</li>
              ))}
            </ul>
          </div>
        ))}
      </Box>
    </Container>
  );
}
