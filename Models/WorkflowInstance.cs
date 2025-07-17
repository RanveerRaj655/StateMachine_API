namespace State_MachineAPI.Models;

public class WorkflowInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DefinitionId { get; set; } = string.Empty;
    public string CurrentStateId { get; set; } = string.Empty;

    public List<WorkflowHistoryEntry> History { get; set; } = new();
}

public class WorkflowHistoryEntry
{
    public string ActionId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
