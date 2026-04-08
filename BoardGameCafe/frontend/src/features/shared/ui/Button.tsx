import React from "react";

type ButtonProps = React.ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: "primary" | "secondary" | "outline";
  size?: "sm" | "md" | "lg";
};

const Button = ({
  variant = "primary",
  size = "md",
  children,
  ...props
}: ButtonProps) => {
  // Base styling
  const base = "font-medium rounded focus:outline-none transition";

  // Variants
  const variants: Record<string, string> = {
    primary: "bg-blue-900 text-white hover:bg-blue-800",
    secondary: "bg-gray-200 text-gray-900 hover:bg-gray-300",
    outline: "border border-blue-900 text-blue-900 hover:bg-blue-50",
  };

  // Sizes
  const sizes: Record<string, string> = {
    sm: "px-3 py-1 text-sm",
    md: "px-6 py-3 text-base",
    lg: "px-8 py-4 text-lg",
  };

  return (
    <button className={`${base} ${variants[variant]} ${sizes[size]}`} {...props}>
      {children}
    </button>
  );
};

export default Button;