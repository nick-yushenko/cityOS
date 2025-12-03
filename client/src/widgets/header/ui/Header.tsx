"use client";

import { AppBar, Toolbar, Typography, Button, Box, Container } from "@mui/material";
import LocationCityRoundedIcon from "@mui/icons-material/LocationCityRounded";
import Link from "next/link";
import { usePathname } from "next/navigation";
import styles from "./Header.module.scss";

const navItems = [
  { label: "Финансы", href: "/finances" },
  { label: "Бюджет", href: "/budget" },
];

export function Header() {
  const pathname = usePathname();

  return (
    <AppBar position="static" className={styles.appBar}>
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          <LocationCityRoundedIcon className={styles.logo} />
          <Typography variant="h6" className={styles.title} sx={{ mr: 2 }}>
            CityOS
          </Typography>

          <Box className={styles.nav}>
            {navItems.map((item) => (
              <Button
                key={item.href}
                component={Link}
                href={item.href}
                className={`${styles.navButton} ${pathname === item.href ? styles.active : ""}`}
              >
                {item.label}
              </Button>
            ))}
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
}
