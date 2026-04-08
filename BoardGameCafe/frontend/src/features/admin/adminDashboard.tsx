import { Link } from "react-router-dom";

const AdminDashboard = () => {
  return (
    <div className="p-8">
      <h1 className="text-3xl font-bold mb-6 text-blue-900">
        Admin Dashboard
      </h1>

      <p className="mb-6 text-gray-700">
        Velg en administrasjonsfunksjon:
      </p>

      <div className="flex flex-col gap-4 max-w-sm">

        <Link
          to="/admin/add-game-copy"
          className="bg-green-600 text-white p-3 rounded-lg text-center hover:bg-green-700"
        >
          Add Game Copy
        </Link>

        <Link
          to="/admin/delete-game-copy"
          className="bg-red-600 text-white p-3 rounded-lg text-center hover:bg-red-700"
        >
          Delete Game Copy
        </Link>

        {/* Future admin tools can be added here */}
        <Link
          to="/games"
          className="bg-blue-700 text-white p-3 rounded-lg text-center hover:bg-blue-800"
        >
          View Games
        </Link>

        <Link
          to="/"
          className="bg-blue-700 text-white p-3 rounded-lg text-center hover:bg-blue-800"
        >
          &larr; Back to Home
        </Link>

      </div>
    </div>
  );
};

export default AdminDashboard;