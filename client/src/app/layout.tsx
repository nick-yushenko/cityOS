import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import { AppRouterCacheProvider } from "@mui/material-nextjs/v15-appRouter";
import { ThemeProvider, CssBaseline } from "@mui/material";
import { darkTheme } from "@/shared/config/theme";
import { Header } from "@/widgets/header";
import "./globals.scss";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "CityOS",
  description: "Система управления городом",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="ru">
      <body className={`${geistSans.variable} ${geistMono.variable}`}>
        <AppRouterCacheProvider>
          <ThemeProvider theme={darkTheme}>
            <CssBaseline />
            <Header />
            <main>{children}</main>
          </ThemeProvider>
        </AppRouterCacheProvider>
      </body>
    </html>
  );
}
