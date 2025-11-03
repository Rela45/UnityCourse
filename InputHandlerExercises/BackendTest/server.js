const express = require("express");
const cors = require("cors");
const bodyParser = require("body-parser");
const db = require("./db");

const app = express();
const port = 8080;

app.use(
cors({
    origin: "*", // Allow all origins for Unity
})
);
app.use(bodyParser.json());

// Fallback tasks in memory (quando MySQL non è disponibile)
let fallbackTasks = [
{
    id: 1,
    title: "Task di Test 1",
    description: "Prima task di esempio senza MySQL",
    completed: false,
},
{
    id: 2,
    title: "Task di Test 2",
    description: "Seconda task di esempio",
    completed: true,
},
{
    id: 3,
    title: "Connessione Unity",
    description: "Test della connessione da Unity",
    completed: false,
},
];

// Test endpoint for connection
app.get("/api/test", (req, res) => {
res.json({
    message: "Server is running!",
    timestamp: new Date().toISOString(),
});
});

// API endpoints for tasks
// GET all tasks
app.get("/api/tasks", async (req, res) => {
try {
    const [tasks] = await db.execute("SELECT * FROM tasks ORDER BY id DESC");
    res.json(tasks);
} catch (error) {
    console.log("⚠️ MySQL not available, using fallback tasks:", error.message);
    // Fallback: restituisci task in memoria
    res.json(fallbackTasks);
}
});

// GET single task
app.get("/api/tasks/:id", async (req, res) => {
try {
    const [tasks] = await db.execute("SELECT * FROM tasks WHERE id = ?", [
    req.params.id,
    ]);
    if (tasks.length === 0) {
    return res.status(404).json({ error: "Task non trovata" });
    }
    res.json(tasks[0]);
} catch (error) {
    res
    .status(500)
    .json({
        error: "Errore nel recupero della task",
        details: error.message,
    });
}
});

// POST new task
app.post("/api/tasks", async (req, res) => {
try {
    const { title, description, completed = false } = req.body;

    if (!title) {
    return res.status(400).json({ error: "Il titolo è obbligatorio" });
    }

    const [result] = await db.execute(
    "INSERT INTO tasks (title, description, completed) VALUES (?, ?, ?)",
    [title, description || "", completed]
    );

    const [newTask] = await db.execute("SELECT * FROM tasks WHERE id = ?", [
    result.insertId,
    ]);
    res.status(201).json(newTask[0]);
} catch (error) {
    console.log(
    "⚠️ MySQL not available for POST, using fallback:",
    error.message
    );
    // Fallback: aggiungi alla lista in memoria
    const { title, description, completed = false } = req.body;

    if (!title) {
    return res.status(400).json({ error: "Il titolo è obbligatorio" });
    }

    const newTask = {
    id: Math.max(...fallbackTasks.map((t) => t.id), 0) + 1,
    title,
    description: description || "",
    completed,
    };

    fallbackTasks.push(newTask);
    res.status(201).json(newTask);
}
});

// PUT update task
app.put("/api/tasks/:id", async (req, res) => {
try {
    const { title, description, completed } = req.body;
    const taskId = req.params.id;

    const [result] = await db.execute(
    "UPDATE tasks SET title = ?, description = ?, completed = ? WHERE id = ?",
    [title, description, completed, taskId]
    );

    if (result.affectedRows === 0) {
    return res.status(404).json({ error: "Task non trovata" });
    }

    const [updatedTask] = await db.execute("SELECT * FROM tasks WHERE id = ?", [
    taskId,
    ]);
    res.json(updatedTask[0]);
} catch (error) {
    res
    .status(500)
    .json({
        error: "Errore nell'aggiornamento della task",
        details: error.message,
    });
}
});

// DELETE task
app.delete("/api/tasks/:id", async (req, res) => {
  try {
    const [result] = await db.execute("DELETE FROM tasks WHERE id = ?", [
      req.params.id,
    ]);

    if (result.affectedRows === 0) {
      return res.status(404).json({ error: "Task non trovata" });
    }

    res.json({ message: "Task eliminata con successo" });
  } catch (error) {
    res.status(500).json({
      error: "Errore nell'eliminazione della task",
      details: error.message,
    });
  }
});

app.listen(port, () => {
  console.log(`Server Node.js in ascolto su http://localhost:${port}`);
  console.log(`Endpoints disponibili:`);
  console.log(`- GET /api/test - test di connessione rapido`);
  console.log(`- GET /api/tasks - Ottieni tutte le tasks`);
  console.log(`- GET /api/tasks/:id - Ottieni una task specifica`);
  console.log(`- POST /api/tasks - Crea una nuova task`);
  console.log(`- PUT /api/tasks/:id - Aggiorna una task`);
  console.log(`- DELETE /api/tasks/:id - Elimina una task`);
});
