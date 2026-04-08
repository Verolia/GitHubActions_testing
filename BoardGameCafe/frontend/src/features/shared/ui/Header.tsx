import { Link } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

const Header = () => {
  const { role, logout } = useAuth();


  return (
    <header className="bg-white shadow">
      <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">
        <Link to="/" className="text-2xl font-bold text-blue-900">
          Board Game Café
        </Link>

        {/* TODO: Remove this - Role text for debugging purposes */}
        <span className="text-gray-600 italic text-sm">
          ({role.charAt(0).toUpperCase() + role.slice(1)})
        </span>

        <nav className="flex items-center gap-4">
          <Link to="/games" className="text-gray-700 hover:text-blue-900">
            Games
          </Link>
          <Link to="/reservations/new" className="text-gray-700 hover:text-blue-900">
            Booking
          </Link>

          {role === "guest" && (
            <>
              <Link
                to="/login"
                className="bg-blue-900 text-white px-4 py-2 rounded hover:bg-blue-800"
              >
                Login
              </Link>
              <Link
                to="/register"
                className="border border-blue-900 text-blue-900 px-4 py-2 rounded hover:bg-blue-50"
              >
                Register
              </Link>
              {/* TODO: Remove temporary Admin Dashboard button once login is functional */}
              <Link
                to="/admin"
                className="bg-red-600 text-white px-3 py-2 rounded hover:bg-red-700"
              >
                TEMP: Admin Dashboard
              </Link>
            </>
          )}

          {role === "customer" && (
            <>
              <Link to="/me/profile" className="text-gray-700 hover:text-blue-900">
                My Profile
              </Link>
              <button
                onClick={logout}
                className="bg-gray-700 text-white px-3 py-2 rounded hover:bg-gray-600"
              >
                Logout
              </button>
            </>
          )}

          {role === "admin" && (
            <>
              <Link to="/admin" className="text-gray-700 hover:text-blue-900">
                Admin Dashboard
              </Link>
              <button
                onClick={logout}
                className="bg-gray-700 text-white px-3 py-2 rounded hover:bg-gray-600"
              >
                Logout
              </button>
            </>
          )}
        </nav>
      </div>
    </header>
  );
};

export default Header;