import React from "react";

type Props = {
  field: {
    name: string,
    value: string,
    onChange: (SyntheticEvent<HTMLInputElement>) => typeof undefined,
    onBlur: (SyntheticEvent<HTMLInputElement>) => typeof undefined
  },
  id: string,
  label: string,
  className: string,
  about: string,
  props: {}
};

const RadioButton = ({
  field: { name, value, onChange, onBlur },
  id,
  label,
  className,
  about,
  ...props
}: Props) => {
  return (
    <div>
      <input
        name={name}
        id={id}
        type="radio"
        value={id}
        checked={id === value}
        onChange={onChange}
        onBlur={onBlur}
        {...props}
      />
      <label htmlFor={id}>{label}</label>
      <p>{about}</p>
    </div>
  );
};

export default RadioButton;
