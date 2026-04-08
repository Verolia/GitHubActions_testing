import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Button from "../../../shared/ui/Button"; 
import { useAuth } from "../../../shared/auth/AuthContext"; 
import type { UserRole } from "../../../shared/auth/auth"; 

interface RegistrationData {
  Name: string;
  Email: string;
  Password: string;
}

const RegisterNewUser = () => {
  const [formData, setFormData] = useState<RegistrationData>({
    Name: "",
    Email: "",
    Password: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();
  const { setRole } = useAuth(); 

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    // --- Frontend validation ---
    if (!formData.Name.trim()) {
      setError("Full name is required.");
      return;
    }

    if (!formData.Email.trim() || !formData.Email.includes("@")) {
      setError("Please enter a valid email address.");
      return;
    }

    if (formData.Password.length < 6) {
      setError("Password must be at least 6 characters.");
      return;
    }

    setLoading(true);

    try {
      const payload = {
        Name: formData.Name,
        Email: formData.Email,
        Password: formData.Password,
      };
      
      const response = await fetch("http://localhost:8080/api/users/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      const data = await response.json(); 

      if (!response.ok) {
        setError(data.errors?.[0] || "Registration failed");
        return;
      }

      const token = data.token;
      const role = (data.role as string).toLowerCase() as UserRole;

      localStorage.setItem("token", token);
      localStorage.setItem("role", role);
      setRole(role);

      navigate("/");
    } catch (err: any) {
      setError(err.message || "Registration failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4 md:px-8">
      <div className="w-full max-w-2xl bg-white rounded-xl shadow-lg p-6 md:p-12">
        <h2 className="text-3xl md:text-4xl font-bold text-center text-blue-900 mb-8">
          Create Your Account
        </h2>

        {error && (
          <p className="text-red-600 text-center font-medium mb-4">{error}</p>
        )}

        <form className="space-y-6" onSubmit={handleSubmit}>
          <div className="flex flex-col space-y-2">
            <label htmlFor="Name" className="text-sm font-medium">Full Name</label>
            <input
              id="Name"
              name="Name"
              type="text"
              value={formData.Name}
              onChange={handleChange}
              placeholder="John Doe"
              className="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 w-full"
              required
            />
          </div>

          <div className="flex flex-col space-y-2">
            <label htmlFor="Email" className="text-sm font-medium">Email</label>
            <input
              id="Email"
              name="Email"
              type="email"
              value={formData.Email}
              onChange={handleChange}
              placeholder="you@example.com"
              className="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 w-full"
              required
            />
          </div>

          <div className="flex flex-col space-y-2">
            <label htmlFor="Password" className="text-sm font-medium">Password</label>
            <input
              id="Password"
              name="Password"
              type="password"
              value={formData.Password}
              onChange={handleChange}
              placeholder="••••••••"
              className="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 w-full"
              required
            />
          </div>

          <Button type="submit" variant="primary" size="lg" className="w-full">
            {loading ? "Registering..." : "Register"}
          </Button>
        </form>

        <p className="text-sm text-gray-500 mt-6 text-center">
          Already have an account? <span className="text-blue-900 font-medium"><a href="/login">Log in</a></span>
        </p>
      </div>
    </div>
  );
};

export default RegisterNewUser;