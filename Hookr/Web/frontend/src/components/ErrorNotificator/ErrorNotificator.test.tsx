import React from "react";
import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import ErrorNotificator from "./ErrorNotificator";

describe("<ErrorNotificator />", () => {
  test("it should mount", () => {
    render(<ErrorNotificator />);

    const errorNotificator = screen.getByTestId("ErrorNotificator");

    expect(errorNotificator).toBeInTheDocument();
  });
});
