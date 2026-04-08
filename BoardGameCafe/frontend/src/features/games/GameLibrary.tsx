import { useEffect, useState } from "react";
import Card from "../shared/ui/Card";
import Button from "../shared/ui/Button";

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

const GameLibrary = () => {
  const [games, setGames] = useState<Game[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch("http://localhost:8080/games", {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer Admin`,
      },
    })
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch games");
        return res.json();
      })
      .then((data: Game[]) => setGames(data))
      .catch((err) => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="flex flex-col md:flex-row min-h-[80vh] p-6 gap-6">
      
      {/* Sidebar */}
      <aside className="w-full md:w-64 bg-white p-4 rounded shadow-md">
        <h2 className="text-lg font-semibold mb-4">Filters & Search</h2>
        <div className="flex flex-col gap-4">
          <input
            type="text"
            placeholder="Search games..."
            className="border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <select className="border border-gray-300 rounded px-3 py-2">
            <option>All Players</option>
            <option>1-2 Players</option>
            <option>3-4 Players</option>
            <option>5+ Players</option>
          </select>
          <select className="border border-gray-300 rounded px-3 py-2">
            <option>All Complexity</option>
            <option>Easy</option>
            <option>Medium</option>
            <option>Hard</option>
          </select>
          <select className="border border-gray-300 rounded px-3 py-2">
            <option>All Playtime</option>
            <option>Under 30 min</option>
            <option>30-60 min</option>
            <option>60+ min</option>
          </select>
          <Button variant="primary" size="md">Apply Filters</Button>
        </div>
      </aside>

      {/* Game Grid */}
      <main className="flex-1 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {loading && <p>Loading games...</p>}
        {error && <p className="text-red-500">Error: {error}</p>}
        {!loading && !error && games.map((game) => (
          <Card key={game.id} className="hover:shadow-lg transition">
            {game.imageUrl ? (
              <img
                src={game.imageUrl}
                alt={game.title}
                className="w-full h-48 object-cover rounded-t-lg"
              />
            ) : (
              <div className="w-full h-48 bg-gray-200 flex items-center justify-center rounded-t-lg">
                No image
              </div>
            )}
            <div className="p-4">
              <h3 className="font-semibold text-lg mb-2">{game.title}</h3>
              <p className="text-gray-700 text-sm mb-2">{game.description}</p>
              <p className="text-gray-500 text-sm">
                Players: {game.maxPlayers} | Playtime: {game.playtimeMin} min | Complexity: {game.complexity}
              </p>
            </div>
          </Card>
        ))}
      </main>
    </div>
  );
};

export default GameLibrary;