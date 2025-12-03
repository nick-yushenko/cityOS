"use client";

import { createTheme } from "@mui/material/styles";

export const darkTheme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: "#90caf9",
    },
    secondary: {
      main: "#ce93d8",
    },
    background: {
      default: "#0a0a0a",
      paper: "#1a1a1a",
    },
  },
  typography: {
    fontFamily: "var(--font-geist-sans), sans-serif",
  },
  components: {
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 12,
          border: "1px solid rgba(255,255,255,0.1)",
        },
      },
    },
  },
});
