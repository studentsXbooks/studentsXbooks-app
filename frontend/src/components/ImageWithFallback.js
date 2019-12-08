import React from "react";
import FallBackImage from "../images/fallBackImage.jpg";

type Props = {
  src: string,
  alt: string,
  fallBack?: string
};

const ImageWithFallback = ({ src, alt, fallBack, ...props }: Props) => (
  <img
    src={src || ""}
    alt={alt}
    onError={e => {
      e.currentTarget.src = fallBack;
    }}
    {...props}
  />
);

ImageWithFallback.defaultProps = {
  fallBack: FallBackImage
};

export default ImageWithFallback;
