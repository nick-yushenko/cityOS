"use client";

import { Button as MUIButton, ButtonProps as MUIButtonProps } from "@mui/material";
import React from "react";

interface Props extends Omit<MUIButtonProps, "onClick"> {
  onClick?: () => void;
}

export const Button: React.FC<Props> = ({ onClick, children, ...rest }) => {
  return (
    <MUIButton onClick={onClick} {...rest}>
      {children}
    </MUIButton>
  );
};
