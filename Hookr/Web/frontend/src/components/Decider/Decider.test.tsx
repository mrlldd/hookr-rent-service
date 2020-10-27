import React from "react";
import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import Decider from "./Decider";

describe("<Decider />", () => {
  test("it should mount", () => {
    render(<Decider />);

    const decider = screen.getByTestId("Decider");

    expect(decider).toBeInTheDocument();
  });
});
