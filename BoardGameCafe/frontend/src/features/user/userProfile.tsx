import { useState } from "react";
import Button from "../shared/ui/Button";

const UserProfile = () => {
  // TODO: Replace temporary mock data with actual user profile data
  const [profile, setProfile] = useState({
    name: "John Doe",
    email: "john@example.com",
    password: "••••••••"
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setProfile({ ...profile, [e.target.name]: e.target.value });
  };

  const handleSave = () => {
    alert("Profile updated (mock)!");
  };

  return (
    <div className="max-w-3xl mx-auto p-6 space-y-8">
      <h1 className="text-3xl font-bold text-blue-900">My Profile</h1>

      <div className="bg-white p-6 rounded-xl shadow space-y-4">
        <div className="flex flex-col space-y-2">
          <label className="text-sm font-medium">Name</label>
          <input
            type="text"
            name="name"
            value={profile.name}
            onChange={handleChange}
            className="border rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div className="flex flex-col space-y-2">
          <label className="text-sm font-medium">Email</label>
          <input
            type="email"
            name="email"
            value={profile.email}
            onChange={handleChange}
            className="border rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div className="flex flex-col space-y-2">
          <label className="text-sm font-medium">Password</label>
          <input
            type="password"
            name="password"
            value={profile.password}
            onChange={handleChange}
            className="border rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <Button variant="primary" size="md" onClick={handleSave}>
          Save Changes
        </Button>
      </div>

      {/* Optional sections */}
      <div className="grid md:grid-cols-3 gap-6">
        <div className="bg-white p-4 rounded-xl shadow">
          <h3 className="font-semibold text-lg mb-2">Favorite Games</h3>
          <p className="text-gray-600">Your favorite games will appear here (mock)</p>
        </div>
        <div className="bg-white p-4 rounded-xl shadow">
          <h3 className="font-semibold text-lg mb-2">Rated Games</h3>
          <p className="text-gray-600">Your rated games will appear here (mock)</p>
        </div>
        <div className="bg-white p-4 rounded-xl shadow">
          <h3 className="font-semibold text-lg mb-2">Previous Bookings</h3>
          <p className="text-gray-600">Your previous bookings will appear here (mock)</p>
        </div>
      </div>
    </div>
  );
};

export default UserProfile;