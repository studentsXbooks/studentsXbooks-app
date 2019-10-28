import React from "react";

const RadioButton = ({
  field: { name, value, onChange, onBlur },
  id,
  label,
  className,
  about,
  ...props
}) => {
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
