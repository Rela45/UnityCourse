using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[Serializable]
public class TaskEs
{
    public int id;
    public string title;
    public string description;
    public bool completed;

    public TaskEs(string title, string description, bool completed = false)
    {
        this.title = title;
        this.description = description;
        this.completed = completed;
    }
}

[Serializable]
public class TaskList
{
    public TaskEs[] tasks;

    public TaskList(TaskEs[] tasks)
    {
        this.tasks = tasks;
    }

    public List<TaskEs> ToList()
    {
        if (tasks == null)
            return new List<TaskEs>();
        return new List<TaskEs>(tasks);
    }
}

[Serializable]
public class TaskResponseEs
{
    public string error;
    public string message;
    public string details;
}

public class TaskManagerEs : MonoBehaviour
{
    public string baseUrl = "http://localhost:8080/api/tasks";
    public bool enableDebugLogs = true;
    public UnityAction<string> OnError;
    public UnityAction<List<TaskEs>> OnTasksLoaded;
    public UnityAction<TaskEs> OnTaskCreated;
    public UnityAction<TaskEs> OnTaskUpdated;
    public UnityAction<string> OnTaskDeleted;
    private void Start()
    {
        // Test connection on start
        StartCoroutine(TestConnection());
    }
    public void TestConnectionManual()
    {
        StartCoroutine(TestConnection());
    }
    public void LoadAllTasks()
    {
        StartCoroutine(GetAllTasksCoroutine());
    }

    public void CreateTask(string title, string description, bool completed = false)
    {
        TaskEs newTask = new TaskEs(title, description, completed);
        StartCoroutine(CreateTaskCoroutine(newTask));
    }
    public void ToggleTaskCompletion(TaskEs task)
    {
        UpdateTask(task.id, task.title, task.description, task.completed);
    }
    public void UpdateTask(int taskId, string title, string description, bool completed)
    {
        TaskEs updatedTask = new TaskEs(title, description, completed);
        updatedTask.id = taskId;
        StartCoroutine(UpdateTaskCoroutine(updatedTask));
    }
    public void DeleteTask(int taskId)
    {
        StartCoroutine(DeleteTaskCoroutine(taskId));
    }


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
                        OnTasksLoaded?.Invoke(new List<TaskEs>());
                        yield break;
                    }

                    // Verifica che sia un array JSON valido
                    if (!jsonResponse.Trim().StartsWith("["))
                    {
                        if (enableDebugLogs)
                            Debug.LogWarning($"Response is not an array: {jsonResponse}");
                        OnTasksLoaded?.Invoke(new List<TaskEs>());
                        yield break;
                    }

                    // JsonUtility non supporta direttamente array/liste, quindi wrappimo
                    string wrappedJson = "{\"tasks\":" + jsonResponse + "}";
                    TaskList wrapper = JsonUtility.FromJson<TaskList>(wrappedJson);

                    // Verifica che il wrapper e l'array non siano null
                    List<TaskEs> tasks = new List<TaskEs>();
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

    private IEnumerator CreateTaskCoroutine(TaskEs task)
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
                    TaskEs createdTask = JsonUtility.FromJson<TaskEs>(jsonResponse);

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

    private IEnumerator UpdateTaskCoroutine(TaskEs task)
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
                    TaskEs updatedTask = JsonUtility.FromJson<TaskEs>(jsonResponse);

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

    private void HandleRequestError(UnityWebRequest request, string operation)
    {
        string errorMessage = $"Error {operation}: {request.error}";

        if (request.downloadHandler != null && !string.IsNullOrEmpty(request.downloadHandler.text))
        {
            try
            {
                TaskResponseEs errorResponse = JsonUtility.FromJson<TaskResponseEs>(request.downloadHandler.text);
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
}
