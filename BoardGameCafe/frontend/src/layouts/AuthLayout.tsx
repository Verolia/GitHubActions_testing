import { Outlet } from "react-router-dom";
import { Link } from "react-router-dom";

const AuthLayout = () => {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50 px-4 md:px-8">
      {/* Back to Home Link */}
      <div className="self-start mb-4 w-full max-w-md">
        <Link to="/" className="text-blue-900 hover:underline font-medium">
          &larr; Back to Home
        </Link>
      </div>

      {/* Auth form content */}
      <div className="w-full max-w-md">
        <Outlet />
      </div>
    </div>
  );
};

export default AuthLayout;