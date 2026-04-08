import { useState } from "react";
import { useNavigate } from "react-router-dom";

interface Game {
  id: string;
  title: string;
}

interface Table {
  id: string;
  name: string;
  capacity: number;
}

const dummyGames: Game[] = [
  { id: "1", title: "Catan" },
  { id: "2", title: "Monopoly" },
  { id: "3", title: "Ticket to Ride" },
];

const dummyTables: Table[] = [
  { id: "1", name: "Table 1", capacity: 4 },
  { id: "2", name: "Table 2", capacity: 6 },
];

const ReservationWizard = () => {
  const [step, setStep] = useState(1);
  const navigate = useNavigate();

  // Wizard state
  const [partySize, setPartySize] = useState(1);
  const [date, setDate] = useState("");
  const [time, setTime] = useState("");
  const [tableId, setTableId] = useState("");
  const [selectedGames, setSelectedGames] = useState<string[]>([]);

  const nextStep = () => setStep(step + 1);
  const prevStep = () => setStep(step - 1);

  const toggleGameSelection = (id: string) => {
    setSelectedGames((prev) =>
      prev.includes(id) ? prev.filter((g) => g !== id) : [...prev, id]
    );
  };

  return (
    <div className="p-6 max-w-2xl mx-auto">
      <h1 className="text-2xl font-bold mb-4">Reservation Wizard</h1>

      {/* Step 1: Party Size */}
      {step === 1 && (
        <div className="space-y-4">
          <label className="block font-medium">Party Size:</label>
          <input
            type="number"
            min={1}
            max={10}
            value={partySize}
            onChange={(e) => setPartySize(Number(e.target.value))}
            className="border p-2 rounded w-24"
          />
          <div className="flex justify-end">
            <button
              onClick={nextStep}
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Next
            </button>
          </div>
        </div>
      )}

      {/* Step 2: Date & Time */}
      {step === 2 && (
        <div className="space-y-4">
          <label className="block font-medium">Date:</label>
          <input
            type="date"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            className="border p-2 rounded"
          />
          <label className="block font-medium">Time:</label>
          <input
            type="time"
            value={time}
            onChange={(e) => setTime(e.target.value)}
            className="border p-2 rounded"
          />
          <div className="flex justify-between mt-4">
            <button
              onClick={prevStep}
              className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500"
            >
              Back
            </button>
            <button
              onClick={nextStep}
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Next
            </button>
          </div>
        </div>
      )}

      {/* Step 3: Table Selection */}
      {step === 3 && (
        <div className="space-y-4">
          <label className="block font-medium">Select Table:</label>
          {dummyTables.map((table) => (
            <div key={table.id}>
              <input
                type="radio"
                id={table.id}
                name="table"
                value={table.id}
                checked={tableId === table.id}
                onChange={(e) => setTableId(e.target.value)}
              />
              <label htmlFor={table.id} className="ml-2">
                {table.name} (Capacity: {table.capacity})
              </label>
            </div>
          ))}
          <div className="flex justify-between mt-4">
            <button
              onClick={prevStep}
              className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500"
            >
              Back
            </button>
            <button
              onClick={nextStep}
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Next
            </button>
          </div>
        </div>
      )}

      {/* Step 4: Game Selection */}
      {step === 4 && (
        <div className="space-y-4">
          <label className="block font-medium">Select Games:</label>
          {dummyGames.map((game) => (
            <div key={game.id}>
              <input
                type="checkbox"
                id={game.id}
                value={game.id}
                checked={selectedGames.includes(game.id)}
                onChange={() => toggleGameSelection(game.id)}
              />
              <label htmlFor={game.id} className="ml-2">
                {game.title}
              </label>
            </div>
          ))}
          <div className="flex justify-between mt-4">
            <button
              onClick={prevStep}
              className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500"
            >
              Back
            </button>
            <button
              onClick={nextStep}
              className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Next
            </button>
          </div>
        </div>
      )}

      {/* Step 5: Review & Pay */}
      {step === 5 && (
        <div className="space-y-4">
          <h2 className="font-bold">Review your reservation</h2>
          <p>Party Size: {partySize}</p>
          <p>Date: {date}</p>
          <p>Time: {time}</p>
          <p>Table: {dummyTables.find((t) => t.id === tableId)?.name}</p>
          <p>
            Games:{" "}
            {selectedGames
              .map((id) => dummyGames.find((g) => g.id === id)?.title)
              .join(", ")}
          </p>
          <div className="flex justify-between mt-4">
            <button
              onClick={prevStep}
              className="bg-gray-400 text-white px-4 py-2 rounded hover:bg-gray-500"
            >
              Back
            </button>
            <button
              onClick={() => {
                alert("Reservation submitted! (frontend only)");
                navigate("/me/dashboard");
              }}
              className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
            >
              Confirm & Pay
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default ReservationWizard;