import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import GameLibrary from "./features/games/GameLibrary";
import ReservationWizard from "./features/user/reservations/makeReservation/makeReservation";
import RegisterNewUser from "./features/user/userProfile/register/registerNewUser";
import LoginForm from "./features/user/userProfile/login/loginForm";
import DeleteGameCopyForm from "./features/admin/GameCopies/deleteGameCopy";
import AdminDashboard from "./features/admin/adminDashboard";
import AddGameCopyForm from "./features/admin/GameCopies/addGameCopy";
import PublicLayout from "./layouts/PublicLayout";
import AuthLayout from "./layouts/AuthLayout";
import AdminLayout from "./layouts/AdminLayout";
import UserProfile from "./features/user/userProfile";

// Main Routes
const AppRoutes = () => {
  return (
    <Routes>
      {/* ---------------- Public Pages ---------------- */}
      <Route path="/" element={<PublicLayout />}>
        <Route index element={<Home />} />
        <Route path="games" element={<GameLibrary />} />
      </Route>

      {/* ---------------- Auth Pages (Login/Register) ---------------- */}
      <Route element={<AuthLayout />}>
        <Route path="login" element={<LoginForm />} />
        <Route path="register" element={<RegisterNewUser />} />
      </Route>

      {/* ---------------- Customer Pages ---------------- */}
      <Route element={<PublicLayout />}>
        {/* Landing page as logged-in customer */}
        <Route path="me/dashboard" element={<Home />} />
        <Route path="me/profile" element={<UserProfile />} />
        <Route path="reservations/new" element={<ReservationWizard />} />
      </Route>

      {/* ---------------- Admin Pages ---------------- */}
      <Route element={<AdminLayout />}>
        <Route path="admin" element={<AdminDashboard />} />
        <Route path="admin/delete-game-copy" element={<DeleteGameCopyForm />} />
        <Route path="admin/add-game-copy" element={<AddGameCopyForm />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;