import { Outlet } from "react-router-dom";
import Header from "../features/shared/ui/Header";
import Footer from "../features/shared/ui/Footer";

const CustomerLayout = () => {
  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <Header />
      <main className="flex-grow">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
};

export default CustomerLayout;