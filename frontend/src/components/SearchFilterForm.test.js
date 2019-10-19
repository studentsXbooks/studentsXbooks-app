import { render } from "@testing-library/react";
import React from "react";
import SearchFilterForm from "./SearchFilterForm";

it("Renders when passed required props", () => {
  const { container } = render(<SearchFilterForm basePath="/something" />);

  expect(container).toBeDefined();
});
