import React from "react";
import { render, fireEvent, cleanup, wait } from "@testing-library/react";
import { Field } from "formik";
import MultiStepForm from "./MultiStepForm";

afterEach(cleanup);

it("Renders when passed all required props", () => {
  const { container } = render(
    <MultiStepForm
      steps={[{ name: "Listing Details", inputGroup: <div>uwu</div> }]}
      onComplete={jest.fn()}
      reviewComponent={() => <div>review</div>}
    />
  );

  expect(container).toBeDefined();
});

it("Passing in steps of 1, renders with next button", () => {
  const { getByText } = render(
    <MultiStepForm
      steps={[{ name: "Listing Details", inputGroup: <div>uwu</div> }]}
      onComplete={jest.fn()}
      reviewComponent={() => <div>review</div>}
    />
  );
  const nextButton = getByText(/Next/);
  expect(nextButton).toBeDefined();
});

it("Clicking on next displays next step component", async () => {
  const { getByText, findByText } = render(
    <MultiStepForm
      steps={[
        { name: "Listing Details", inputGroup: <div>uwu</div> },
        { name: "Contact Info", inputGroup: <div>UniqueText</div> }
      ]}
      onComplete={jest.fn()}
      reviewComponent={() => <div>review</div>}
    />
  );
  fireEvent.submit(getByText(/Next/));
  const nextForm = await findByText(/UniqueText/);
  expect(nextForm).toBeDefined();
});

it("Clicking on next on last step component, displays review component", async () => {
  const { getByText, findByText } = render(
    <MultiStepForm
      steps={[
        { name: "Listing Details", inputGroup: <div>uwu</div> },
        { name: "Contact Info", inputGroup: <div>UniqueText</div> }
      ]}
      onComplete={jest.fn()}
      reviewComponent={() => <div>review</div>}
    />
  );
  fireEvent.submit(getByText(/Next/));
  await findByText(/UniqueText/);
  fireEvent.submit(getByText(/Next/));
  const review = await findByText(/review/);
  expect(review).toBeDefined();
});

it("Clicking on submit during review component being displayed, calls onComplete function", async () => {
  const onCompleteMock = jest.fn();
  const { getByText, findByText } = render(
    <MultiStepForm
      steps={[{ name: "Listing Details", inputGroup: <div>uwu</div> }]}
      onComplete={onCompleteMock}
      reviewComponent={() => <div>review</div>}
    />
  );
  fireEvent.submit(getByText(/Next/));
  const submit = await findByText(/Submit/);
  fireEvent.submit(submit);
  await wait(() => expect(onCompleteMock).toHaveBeenCalledTimes(1));
});
it("Submitting Forms compiles through to final submission and passes compiled data to onComplete function", async () => {
  const onCompleteMock = jest.fn();

  const FakeForm = (
    <label>
      something
      <Field
        name="something"
        id="something"
        component="input"
        placeholder="something"
        type="text"
        label="something"
        required
      />
    </label>
  );
  const { getByText, findByText, getByLabelText } = render(
    <MultiStepForm
      steps={[{ name: "FakeForm", inputGroup: FakeForm }]}
      initialValues={{ something: "" }}
      onComplete={onCompleteMock}
      reviewComponent={() => <div>review</div>}
    />
  );
  fireEvent.change(getByLabelText(/something/), {
    target: { value: "Something" }
  });
  fireEvent.blur(getByLabelText(/something/));
  fireEvent.submit(getByText(/Next/));
  const submit = await findByText(/Submit/);
  fireEvent.submit(submit);
  await wait(() => {
    expect(onCompleteMock).toHaveBeenCalledTimes(1);
    expect(onCompleteMock).toHaveBeenCalledWith(
      { something: "Something" },
      expect.any(Object)
    );
  });
});

it("Review component passed in prop of values of current form", async () => {
  const onCompleteMock = jest.fn();

  const FakeForm = (
    <label>
      something
      <Field
        name="something"
        id="something"
        component="input"
        placeholder="something"
        type="text"
        label="something"
        required
      />
    </label>
  );
  // $FlowFixMe
  const Review = ({ values: { something } }) => (
    <div>Something: {something}</div>
  );
  const { getByText, findByText, getByLabelText } = render(
    <MultiStepForm
      steps={[{ name: "FakeForm", inputGroup: FakeForm }]}
      initialValues={{ something: "" }}
      onComplete={onCompleteMock}
      reviewComponent={values => <Review values={values} />}
    />
  );
  fireEvent.change(getByLabelText(/something/), {
    target: { value: "Something" }
  });
  fireEvent.blur(getByLabelText(/something/));
  fireEvent.submit(getByText(/Next/));
  const review = await findByText(/Something: Something/);
  expect(review).toBeDefined();
});

describe("Stepper being shown", () => {
  it("First step is active and name is shown in stepper", () => {
    const { getByText } = render(
      <MultiStepForm
        steps={[{ name: "Step 1", inputGroup: <div>FakeText</div> }]}
        onComplete={jest.fn()}
        reviewComponent={() => <div>fake</div>}
      />
    );

    const step1 = getByText(/Step 1/);
    expect(step1).toBeDefined();
  });
  it("Second step is active and name is shown in stepper", () => {
    const { getByText } = render(
      <MultiStepForm
        steps={[
          { name: "Step 1", inputGroup: <div>FakeText</div> },
          { name: "Step 2", inputGroup: <div>FakeText</div> }
        ]}
        onComplete={jest.fn()}
        reviewComponent={() => <div>fake</div>}
      />
    );

    fireEvent.submit(getByText(/Next/));
    const step2 = getByText(/Step 2/);
    expect(step2).toBeDefined();
  });
});
