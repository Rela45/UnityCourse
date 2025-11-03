using UnityEngine;


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[Serializable]
public class Task
{
    public int id;
    public string title;
    public string description;
    public bool completed;
    
    public Task(string title, string description, bool completed = false)
    {
        this.title = title;
        this.description = description;
        this.completed = completed;
    }
}

[Serializable]
public class TaskResponse
{
    public string error;
    public string message;
    public string details;
}

[Serializable]
public class TaskListWrapper
{
    public Task[] tasks;
    
    public TaskListWrapper(Task[] tasks)
    {
        this.tasks = tasks;
    }
    
    public List<Task> ToList()
    {
        if (tasks == null)
            return new List<Task>();
        return new List<Task>(tasks);
    }
}

public class TaskManager : MonoBehaviour
{
    [Header("Server Configuration")]
    public string baseUrl = "http://localhost:8080/api/tasks";
    
    [Header("Events")]
    public UnityAction<List<Task>> OnTasksLoaded;
    public UnityAction<Task> OnTaskCreated;
    public UnityAction<Task> OnTaskUpdated;
    public UnityAction<string> OnTaskDeleted;
    public UnityAction<string> OnError;
    
    [Header("Debug")]
    public bool enableDebugLogs = true;
    
    private void Start()
    {
        // Test connection on start
        StartCoroutine(TestConnection());
    }
    
    #region Public Methods
    
    /// <summary>
    /// Carica tutte le tasks dal server
    /// </summary>
    public void LoadAllTasks()
    {
        StartCoroutine(GetAllTasksCoroutine());
    }
    
    /// <summary>
    /// Carica una task specifica per ID
    /// </summary>
    public void LoadTask(int taskId)
    {
        StartCoroutine(GetTaskCoroutine(taskId));
    }
    
    /// <summary>
    /// Crea una nuova task
    /// </summary>
    public void CreateTask(string title, string description, bool completed = false)
    {
        Task newTask = new Task(title, description, completed);
        StartCoroutine(CreateTaskCoroutine(newTask));
    }
    
    /// <summary>
    /// Aggiorna una task esistente
    /// </summary>
    public void UpdateTask(int taskId, string title, string description, bool completed)
    {
        Task updatedTask = new Task(title, description, completed);
        updatedTask.id = taskId;
        StartCoroutine(UpdateTaskCoroutine(updatedTask));
    }
    
    /// <summary>
    /// Elimina una task
    /// </summary>
    public void DeleteTask(int taskId)
    {
        StartCoroutine(DeleteTaskCoroutine(taskId));
    }
    
    /// <summary>
    /// Segna una task come completata/non completata
    /// </summary>
    public void ToggleTaskCompletion(Task task)
    {
        UpdateTask(task.id, task.title, task.description, !task.completed);
    }
    
    /// <summary>
    /// Test di connessione manuale
    /// </summary>
    [ContextMenu("Test Connection")]
    public void TestConnectionManual()
    {
        StartCoroutine(TestConnection());
    }
    
    /// <summary>
    /// Test caricamento tasks manuale
    /// </summary>
    [ContextMenu("Test Load Tasks")]
    public void TestLoadTasksManual()
    {
        LoadAllTasks();
    }
    
    #endregion
    
    #region Coroutines
    
    private IEnumerator TestConnection()
    {
        if (enableDebugLogs)
            Debug.Log("Testing connection to server...");
            
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl))
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (enableDebugLogs)
                    Debug.Log("✅ Connection to server successful!");
            }
            else
            {
                string error = $"❌ Connection failed: {request.error} on {baseUrl}";
                if (enableDebugLogs)
                    Debug.LogError(error);
                OnError?.Invoke(error);
            }
        }
    }
    
    private IEnumerator GetAllTasksCoroutine()
    {
        if (enableDebugLogs)
            Debug.Log("Loading all tasks...");
            
        using (UnityWebRequest request = UnityWebRequest.Get(baseUrl))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    
                    if (enableDebugLogs)
                        Debug.Log($"Raw JSON response: {jsonResponse}");
                    
                    // Controlla se la risposta è vuota o null
                    if (string.IsNullOrEmpty(jsonResponse) || jsonResponse.Trim() == "null")
                    {
                        if (enableDebugLogs)
                            Debug.Log("✅ Empty response, returning empty task list");
                        OnTasksLoaded?.Invoke(new List<Task>());
                        yield break;
                    }
                    
                    // Verifica che sia un array JSON valido
                    if (!jsonResponse.Trim().StartsWith("["))
                    {
                        if (enableDebugLogs)
                            Debug.LogWarning($"Response is not an array: {jsonResponse}");
                        OnTasksLoaded?.Invoke(new List<Task>());
                        yield break;
                    }
                    
                    // JsonUtility non supporta direttamente array/liste, quindi wrappimo
                    string wrappedJson = "{\"tasks\":" + jsonResponse + "}";
                    TaskListWrapper wrapper = JsonUtility.FromJson<TaskListWrapper>(wrappedJson);
                    
                    // Verifica che il wrapper e l'array non siano null
                    List<Task> tasks = new List<Task>();
                    if (wrapper != null && wrapper.tasks != null)
                    {
                        tasks = wrapper.ToList();
                    }
                    
                    if (enableDebugLogs)
                        Debug.Log($"✅ Loaded {tasks.Count} tasks");
                    
                    OnTasksLoaded?.Invoke(tasks);
                }
                catch (Exception e)
                {
                    string error = $"Error parsing tasks: {e.Message}";
                    if (enableDebugLogs)
                        Debug.LogError($"{error} | Raw response: {request.downloadHandler.text}");
                    OnError?.Invoke(error);
                }
            }
            else
            {
                HandleRequestError(request, "loading tasks");
            }
        }
    }
    
    private IEnumerator GetTaskCoroutine(int taskId)
    {
        if (enableDebugLogs)
            Debug.Log($"Loading task {taskId}...");
            
        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/{taskId}"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    Task task = JsonUtility.FromJson<Task>(jsonResponse);
                    
                    if (enableDebugLogs)
                        Debug.Log($"✅ Loaded task: {task.title}");
                    
                    // Per una singola task, la passiamo in una lista
                    OnTasksLoaded?.Invoke(new List<Task> { task });
                }
                catch (Exception e)
                {
                    string error = $"Error parsing task: {e.Message}";
                    if (enableDebugLogs)
                        Debug.LogError(error);
                    OnError?.Invoke(error);
                }
            }
            else
            {
                HandleRequestError(request, $"loading task {taskId}");
            }
        }
    }
    
    private IEnumerator CreateTaskCoroutine(Task task)
    {
        if (enableDebugLogs)
            Debug.Log($"Creating task: {task.title}");
            
        string jsonData = JsonUtility.ToJson(task);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(baseUrl, ""))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    Task createdTask = JsonUtility.FromJson<Task>(jsonResponse);
                    
                    if (enableDebugLogs)
                        Debug.Log($"✅ Task created: {createdTask.title} (ID: {createdTask.id})");
                    
                    OnTaskCreated?.Invoke(createdTask);
                }
                catch (Exception e)
                {
                    string error = $"Error parsing created task: {e.Message}";
                    if (enableDebugLogs)
                        Debug.LogError(error);
                    OnError?.Invoke(error);
                }
            }
            else
            {
                HandleRequestError(request, "creating task");
            }
        }
    }
    
    private IEnumerator UpdateTaskCoroutine(Task task)
    {
        if (enableDebugLogs)
            Debug.Log($"Updating task {task.id}: {task.title}");
            
        string jsonData = JsonUtility.ToJson(task);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        
        using (UnityWebRequest request = UnityWebRequest.Put($"{baseUrl}/{task.id}", bodyRaw))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    Task updatedTask = JsonUtility.FromJson<Task>(jsonResponse);
                    
                    if (enableDebugLogs)
                        Debug.Log($"✅ Task updated: {updatedTask.title}");
                    
                    OnTaskUpdated?.Invoke(updatedTask);
                }
                catch (Exception e)
                {
                    string error = $"Error parsing updated task: {e.Message}";
                    if (enableDebugLogs)
                        Debug.LogError(error);
                    OnError?.Invoke(error);
                }
            }
            else
            {
                HandleRequestError(request, $"updating task {task.id}");
            }
        }
    }
    
    private IEnumerator DeleteTaskCoroutine(int taskId)
    {
        if (enableDebugLogs)
            Debug.Log($"Deleting task {taskId}...");
            
        using (UnityWebRequest request = UnityWebRequest.Delete($"{baseUrl}/{taskId}"))
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                if (enableDebugLogs)
                    Debug.Log($"✅ Task {taskId} deleted");
                
                OnTaskDeleted?.Invoke($"Task {taskId} deleted successfully");
            }
            else
            {
                HandleRequestError(request, $"deleting task {taskId}");
            }
        }
    }
    
    #endregion
    
    #region Helper Methods
    
    private void HandleRequestError(UnityWebRequest request, string operation)
    {
        string errorMessage = $"Error {operation}: {request.error}";
        
        if (request.downloadHandler != null && !string.IsNullOrEmpty(request.downloadHandler.text))
        {
            try
            {
                TaskResponse errorResponse = JsonUtility.FromJson<TaskResponse>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(errorResponse.error))
                {
                    errorMessage += $" - {errorResponse.error}";
                }
            }
            catch
            {
                errorMessage += $" - Raw response: {request.downloadHandler.text}";
            }
        }
        
        if (enableDebugLogs)
            Debug.LogError($"❌ {errorMessage}");
        
        OnError?.Invoke(errorMessage);
    }
    
    #endregion
}

