import React from "react";

export const Button = ({ children, className = "", onClick, ...props }) => {
  return (
    <button
      onClick={onClick}
      className={`px-4 py-2 rounded-md font-medium bg-blue-600 text-white hover:bg-blue-700 transition ${className}`}
      {...props}
    >
      {children}
    </button>
  );
};
