using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TaskItem : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI idText;
    public Toggle completedToggle;
    public Button deleteButton;
    public Button editButton;
    
    [Header("Styling")]
    public Color completedColor = Color.green;
    public Color incompleteColor = Color.white;
    
    private TaskEs currentTask;
    private TaskUi taskUI;
    
    public void Setup(TaskEs task, TaskUi ui)
    {
        currentTask = task;
        taskUI = ui;
        
        UpdateUI();
        SetupEvents();
    }
    
    private void UpdateUI()
    {
        if (currentTask == null) return;
        
        // Update text fields
        if (titleText != null)
            titleText.text = currentTask.title;
            
        if (descriptionText != null)
            descriptionText.text = string.IsNullOrEmpty(currentTask.description) ? "No description" : currentTask.description;
            
        if (idText != null)
            idText.text = $"ID: {currentTask.id}";
            
        // Update completion status
        if (completedToggle != null)
        {
            completedToggle.SetIsOnWithoutNotify(currentTask.completed);
        }
        
        // Update visual style based on completion
        UpdateVisualStyle();
    }
    
    private void UpdateVisualStyle()
    {
        Color targetColor = currentTask.completed ? completedColor : incompleteColor;
        
        if (titleText != null)
        {
            titleText.color = targetColor;
            titleText.fontStyle = currentTask.completed ? FontStyles.Strikethrough : FontStyles.Normal;
        }
        
        if (descriptionText != null)
        {
            descriptionText.color = targetColor;
            descriptionText.alpha = currentTask.completed ? 0.7f : 1f;
        }
    }
    
    private void SetupEvents()
    {
        // Completion toggle
        if (completedToggle != null)
        {
            completedToggle.onValueChanged.RemoveAllListeners();
            completedToggle.onValueChanged.AddListener(OnToggleCompleted);
        }
        
        // Delete button
        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(OnDeleteClicked);
        }
        
        // Edit button (for future expansion)
        if (editButton != null)
        {
            editButton.onClick.RemoveAllListeners();
            editButton.onClick.AddListener(OnEditClicked);
        }
    }
    
    private void OnToggleCompleted(bool isCompleted)
    {
        if (currentTask != null && taskUI != null)
        {
            // Update local task data
            currentTask.completed = isCompleted;
            UpdateVisualStyle();
            
            // Send update to server
            taskUI.ToggleTask(currentTask);
        }
    }
    
    private void OnDeleteClicked()
    {
        if (currentTask != null && taskUI != null)
        {
            // Confirm deletion (optional)
            if (Application.isEditor)
            {
                if (UnityEditor.EditorUtility.DisplayDialog(
                    "Delete Task", 
                    $"Are you sure you want to delete '{currentTask.title}'?", 
                    "Yes", "No"))
                {
                    taskUI.DeleteTask(currentTask.id);
                }
            }
            else
            {
                // In build, delete directly (you might want to add a confirmation UI)
                taskUI.DeleteTask(currentTask.id);
            }
        }
    }
    
    private void OnEditClicked()
    {
        // Placeholder for edit functionality
        Debug.Log($"Edit task: {currentTask.title}");
        // You can implement an edit dialog or inline editing here
    }
    
    public TaskEs GetTask()
    {
        return currentTask;
    }
}
