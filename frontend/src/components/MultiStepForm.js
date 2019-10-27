import type { Node } from "react";
import React, { useState } from "react";
import { Button, Stepper, Step, StepLabel } from "@material-ui/core";
import { Formik, Form } from "formik";

type Props = {
  steps: {
    name: string,
    inputGroup: Node
  }[],
  onComplete: (values: {}, bag: {}) => null,
  reviewComponent: ({}) => Node
};

const MultiStepForm = ({
  steps,
  onComplete,
  reviewComponent,
  ...props
}: Props) => {
  const [step, setStep] = useState(1);

  const displayComponents = steps.map(x => x.inputGroup);

  const toRender = displayComponents[step - 1];
  const isLastStep = (currentStep: number) =>
    currentStep === displayComponents.length + 1;

  const handleSubmit = (values, bag) => {
    if (isLastStep(step)) {
      onComplete(values, bag);
    } else {
      bag.setTouched({});
      bag.setSubmitting(false);
      setStep(step + 1);
    }
  };

  return (
    <Formik onSubmit={handleSubmit} {...props}>
      {({ values }) => (
        <Form>
          <Stepper activeStep={step}>
            {steps
              .map(x => x.name)
              .map(x => (
                <Step key={x}>
                  <StepLabel>{x}</StepLabel>
                </Step>
              ))}
          </Stepper>
          {isLastStep(step) ? reviewComponent(values) : toRender}
          <Button type="submit">{isLastStep(step) ? "Submit" : "Next"}</Button>
        </Form>
      )}
    </Formik>
  );
};

export default MultiStepForm;
