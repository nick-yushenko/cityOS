"use client";

import { TextField, TextFieldProps } from "@mui/material";
import React from "react";

type Props = {
  label: string;
} & TextFieldProps;

export const Input: React.FC<Props> = ({ ...rest }) => {
  return <TextField {...rest} />;
};
