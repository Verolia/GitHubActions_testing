import { Outlet } from "react-router-dom";
import { useAuth } from "../features/shared/auth/AuthContext";


const AdminLayout = () => {
  const { logout } = useAuth();

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      {/* Admin header */}
      <header className="bg-gray-900 text-white p-4 flex justify-between items-center">
        <h1 className="text-xl font-bold">Admin Dashboard</h1>
          <button
            onClick={logout}
            className="bg-gray-700 text-white px-3 py-2 rounded hover:bg-gray-600"
          >
            Logout
          </button>
      </header>

      <main className="flex-grow p-6">
        <Outlet />
      </main>

      <footer className="bg-gray-100 py-4 text-center text-sm text-gray-500">
        © 2026 Board Game Café — Admin
      </footer>
    </div>
  );
};

export default AdminLayout;