import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Card from "../features/shared/ui/Card";
import Button from "../features/shared/ui/Button";

interface Game {
  id: string;
  title: string;
  maxPlayers: number;
  playtimeMin: number;
  complexity: number;
  description: string;
  imageUrl: string;
  createdAt: number;
}

const Home = () => {
  const [games, setGames] = useState<Game[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch("http://localhost:8080/games", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer Admin`, // Replace with actual auth
      },
    })
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch games");
        return res.json();
      })
      .then((data: Game[]) => setGames(data.slice(0, 5))) // preview first 5
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="flex flex-col items-center justify-center min-h-[80vh] px-4 py-12 bg-gray-50 gap-12">

      {/* Hero Section */}
      <section className="text-center max-w-3xl">
        <h1 className="text-4xl md:text-5xl font-bold text-blue-900 mb-6">
          Welcome to Board Game Café
        </h1>
        <p className="text-gray-700 text-lg md:text-xl mb-6">
          Explore our large collection of board games, reserve a table, and enjoy a fun session with friends.
        </p>
      </section>

      {/* Games Preview Carousel */}
      <section className="w-full max-w-6xl">
        <h2 className="text-2xl font-semibold text-blue-900 mb-4">Browse Games</h2>

        {loading && <p>Loading games...</p>}
        {error && <p className="text-red-500">Error: {error}</p>}

        {!loading && !error && (
          <div className="flex overflow-x-auto gap-4 pb-4">
            {games.map((game) => (
              <Card key={game.id} className="min-w-[200px] flex-shrink-0">
                {game.imageUrl ? (
                  <img
                    src={game.imageUrl}
                    alt={game.title}
                    className="rounded-t-lg w-full h-32 object-cover"
                  />
                ) : (
                  <div className="w-full h-32 bg-gray-200 flex items-center justify-center rounded-t-lg">
                    No image
                  </div>
                )}
                <div className="p-2">
                  <h3 className="font-semibold text-lg">{game.title}</h3>
                  <p className="text-gray-600 text-sm">{game.maxPlayers} players | {game.playtimeMin} min</p>
                </div>
              </Card>
            ))}
          </div>
        )}

        <div className="mt-4 flex justify-center">
          <Link to="/games">
            <Button variant="primary" size="md">Show More</Button>
          </Link>
        </div>
      </section>

      <section className="w-full max-w-6xl">
        <h2 className="text-2xl font-semibold text-blue-900 mb-4">Available Timeslots</h2>

        <div className="grid sm:grid-cols-2 md:grid-cols-3 gap-6 mb-6">
          {/* TODO: Get from database, instead of using placeholder slots */}
          {["12:00 PM", "1:30 PM", "3:00 PM", "4:30 PM", "6:00 PM", "7:30 PM"].map((time) => (
            <div
              key={time}
              className="bg-white rounded-xl shadow-md p-4 flex flex-col items-center justify-between hover:shadow-lg transition"
            >
              <p className="text-lg font-semibold mb-2">{time}</p>
              <p className="text-gray-600 mb-4">Table for 2-4 players</p>
              <Link to="/login">
                <button className="bg-blue-900 text-white px-4 py-2 rounded hover:bg-blue-800 transition">
                  Book Table
                </button>
              </Link>
            </div>
          ))}
        </div>

      </section>
    </div>
  );
};

export default Home;