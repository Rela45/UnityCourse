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