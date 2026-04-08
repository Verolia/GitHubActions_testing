import { useState } from "react";

const AddGameCopyForm = () => {
  const [gameId, setGameId] = useState("");
  const [message, setMessage] = useState("");

  async function handleAdd(e: React.FormEvent) {
    e.preventDefault();
    setMessage("");

    try {
      const response = await fetch(
        `http://localhost:8080/api/games/${gameId}/copies`,
        {
          method: "POST",
          headers: {
            "Authorization": "Bearer Admin",
            "Content-Type": "application/json",
          },
        }
      );

      
        console.log("STATUS:", response.status);
        console.log("RAW RESPONSE:", await response.text());


      if (!response.ok) {
        throw new Error("Failed to add game copy");
      }

      setMessage("Game copy added successfully!");
      setGameId("");
    }
    catch (error: any) {
      setMessage("Error: " + error.message);
    }
  }

  return (
    <div className="p-6 max-w-md mx-auto">
      <h2 className="text-2xl font-bold mb-4">Add Game Copy</h2>

      <form onSubmit={handleAdd} className="space-y-4">
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

        <button
          type="submit"
          className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
        >
          Add Game Copy
        </button>
      </form>

      {message && <p className="mt-4">{message}</p>}
    </div>
  );
};

export default AddGameCopyForm;