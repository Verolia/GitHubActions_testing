
export type UserRole = "guest" | "customer" | "admin" | "steward";

/**
 * Reads the role from the JWT token in localStorage.
 * Returns "guest" if no token or invalid token.
 */
export function getRoleFromToken(): UserRole {
  const token = localStorage.getItem("token");
  if (!token) return "guest";

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return (payload.role?.toLowerCase() as UserRole) ?? "guest";
  } catch {
    return "guest";
  }
}