import React from "react";
import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import LoggedInWrapper from "./LoggedInWrapper";

describe("<LoggedInWrapper />", () => {
  test("it should mount", () => {
    render(<LoggedInWrapper />);

    const wrapper = screen.getByTestId("LoggedInWrapper");

    expect(wrapper).toBeInTheDocument();
  });
});
