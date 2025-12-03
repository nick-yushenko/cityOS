import { Container, Typography, Box } from "@mui/material";

export default function Page() {
  return (
    <Container maxWidth="xl">
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom fontWeight={600}>
          Финансы города
        </Typography>
      </Box>
    </Container>
  );
}
