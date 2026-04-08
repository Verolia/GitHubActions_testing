import React from "react";

type CardProps = React.HTMLAttributes<HTMLDivElement> & {
  shadow?: boolean;
  rounded?: boolean;
};

const Card: React.FC<CardProps> = ({
  children,
  shadow = true,
  rounded = true,
  className = "",
  ...props
}) => {
  return (
    <div
      className={`${shadow ? "shadow-xl" : ""} ${
        rounded ? "rounded-xl" : ""
      } bg-white p-6 ${className}`}
      {...props}
    >
      {children}
    </div>
  );
};

export default Card;