import { useState } from "react";

const DeleteGameCopyForm = () => {
  const [gameId, setGameId] = useState("");
  const [gameCopyId, setGameCopyId] = useState("");
  const [message, setMessage] = useState("");

  async function handleDelete(e: React.FormEvent) {
    e.preventDefault();
    setMessage("");

    try {
      const response = await fetch(
        `http://localhost:8080/api/games/${gameId}/copies/${gameCopyId}`,
        {
          method: "DELETE",
          headers: {
            "Authorization": "Bearer Admin",
            "Content-Type": "application/json",
          },
        }
      );

      
        console.log("STATUS:", response.status);
        console.log("RAW RESPONSE:", await response.text());


      if (!response.ok) {
        throw new Error("Failed to delete game copy");
      }

      setMessage("Game copy deleted successfully!");
      setGameId("");
      setGameCopyId("");
    }
    catch (error: any) {
      setMessage("Error: " + error.message);
    }
  }

  return (
    <div className="p-6 max-w-md mx-auto">
      <h2 className="text-2xl font-bold mb-4">Delete Game Copy</h2>

      <form onSubmit={handleDelete} className="space-y-4">
        <div>
          <label className="block font-semibold">GameId (GUID)</label>
          <input
            type="text"
            value={gameId}
            onChange={(e) => setGameId(e.target.value)}
            className="w-full border p-2 rounded"
            placeholder="Enter GameId"
            required
          />
        </div>

        <div>
          <label className="block font-semibold">GameCopyId (GUID)</label>
          <input
            type="text"
            value={gameCopyId}
            onChange={(e) => setGameCopyId(e.target.value)}
            className="w-full border p-2 rounded"
            placeholder="Enter GameCopyId"
            required
          />
        </div>

        <button
          type="submit"
          className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
        >
          Delete Game Copy
        </button>
      </form>

      {message && <p className="mt-4">{message}</p>}
    </div>
  );
};

export default DeleteGameCopyForm;