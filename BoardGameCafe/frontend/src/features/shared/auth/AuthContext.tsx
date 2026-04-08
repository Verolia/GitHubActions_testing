import { createContext, useContext, useState } from "react";
import type { ReactNode } from "react";
import type { UserRole } from "../auth/auth";
import { getRoleFromToken } from "../auth/auth";

type AuthContextType = {
  role: UserRole;
  setRole: (role: UserRole) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextType>({
  role: "guest",
  setRole: () => {},
  logout: () => {},
});

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [role, setRole] = useState<UserRole>(getRoleFromToken().toLowerCase() as UserRole);

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("role");
    setRole("guest"); // updates header immediately
  };

  return (
    <AuthContext.Provider value={{ role, setRole, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);