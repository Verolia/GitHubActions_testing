import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Button from "../../../shared/ui/Button";
import { useAuth } from "../../../shared/auth/AuthContext";
import type { UserRole } from "../../../shared/auth/auth";

export default function LoginForm() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();
  const { setRole } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const res = await fetch("http://localhost:8080/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ EmailAddress: email, Password: password }),
      });

      if (!res.ok) {
        const errData = await res.json().catch(() => ({}));
        throw new Error(errData?.message || "Login failed");
      }

      const data = await res.json();
      console.log("Token:", data.token, "Role:", data.role);

      // Normalize role to lowercase for consistency
      const normalizedRole = (data.role as string).toLowerCase() as UserRole;

      // Save token & role in localStorage
      localStorage.setItem("token", data.token);
      localStorage.setItem("role", normalizedRole);

      // Update context so header updates immediately
      setRole(normalizedRole);

      // Redirect to landing page (header will update based on role)
      navigate("/");

    } catch (err: any) {
      setError(err.message || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4 md:px-8">
      <div className="w-full max-w-md bg-white rounded-xl shadow-lg p-6 md:p-12">
        <h2 className="text-3xl md:text-4xl font-bold text-center text-blue-900 mb-8">
          Log In
        </h2>

        {error && <p className="text-red-600 text-center mb-4">{error}</p>}

        <form className="space-y-6" onSubmit={handleSubmit}>
          <div className="flex flex-col space-y-2">
            <label htmlFor="email" className="text-sm font-medium">
              Email
            </label>
            <input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="you@example.com"
              className="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 w-full"
              required
            />
          </div>

          <div className="flex flex-col space-y-2">
            <label htmlFor="password" className="text-sm font-medium">
              Password
            </label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="••••••••"
              className="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 w-full"
              required
            />
          </div>

          <Button type="submit" variant="primary" size="lg" className="w-full">
            {loading ? "Logging in..." : "Log In"}
          </Button>
        </form>

        <p className="text-sm text-gray-500 mt-6 text-center">
          Don’t have an account?{" "}
          <a href="/register" className="text-blue-900 font-medium">
            Register
          </a>
        </p>

        <p className="text-sm text-gray-400 mt-4 text-center">
          TEMP: login as admin → <a href="/admin" className="text-blue-900">Admin Dashboard</a>
        </p>
      </div>
    </div>
  );
}