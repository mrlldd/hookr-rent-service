import React from "react";
import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import FullPageWrapper from "./FullPageWrapper";

describe("<FullPageWrapper />", () => {
  test("it should mount", () => {
    render(<FullPageWrapper />);

    const fullPageWrapper = screen.getByTestId("FullPageWrapper");

    expect(fullPageWrapper).toBeInTheDocument();
  });
});
