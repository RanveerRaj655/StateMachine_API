using State_MachineAPI.Models;
namespace State_MachineAPI.Services;


public class WorkflowService
{
    private readonly Dictionary<string, WorkflowDefinition> _definitions = new();
    private readonly Dictionary<string, WorkflowInstance> _instances = new();

    public (bool success, string? error) AddDefinition(WorkflowDefinition def)
    {
        if (_definitions.ContainsKey(def.Id))
            return (false, "Duplicate workflow ID.");

        var initialStates = def.States.Count(s => s.IsInitial);
        if (initialStates != 1)
            return (false, "Workflow must have exactly one initial state.");

        var stateIds = def.States.Select(s => s.Id).ToHashSet();
        if (stateIds.Count != def.States.Count)
            return (false, "Duplicate state IDs found.");

        var actionIds = def.Actions.Select(a => a.Id).ToHashSet();
        if (actionIds.Count != def.Actions.Count)
            return (false, "Duplicate action IDs found.");

        foreach (var action in def.Actions)
        {
            if (!stateIds.Contains(action.ToState))
                return (false, $"Invalid toState: {action.ToState}");
            foreach (var from in action.FromStates)
            {
                if (!stateIds.Contains(from))
                    return (false, $"Invalid fromState: {from}");
            }
        }

        _definitions[def.Id] = def;
        return (true, null);
    }

    public WorkflowDefinition? GetDefinition(string id)
    {
        return _definitions.TryGetValue(id, out var def) ? def : null;
    }

    public (bool success, string? error, WorkflowInstance? instance) StartInstance(string defId)
    {
        if (!_definitions.TryGetValue(defId, out var def))
            return (false, "Definition not found.", null);

        var initialState = def.States.FirstOrDefault(s => s.IsInitial && s.Enabled);
        if (initialState == null)
            return (false, "No enabled initial state found.", null);

        var instance = new WorkflowInstance
        {
            DefinitionId = defId,
            CurrentStateId = initialState.Id,
        };

        _instances[instance.Id] = instance;
        return (true, null, instance);
    }

    public (bool success, string? error) ExecuteAction(string instanceId, string actionId)
    {
        if (!_instances.TryGetValue(instanceId, out var instance))
            return (false, "Instance not found.");

        var def = _definitions[instance.DefinitionId];
        var currentState = def.States.FirstOrDefault(s => s.Id == instance.CurrentStateId);

        if (currentState == null || currentState.IsFinal)
            return (false, "Current state is invalid or final.");

        var action = def.Actions.FirstOrDefault(a => a.Id == actionId);
        if (action == null || !action.Enabled)
            return (false, "Action not found or disabled.");

        if (!action.FromStates.Contains(instance.CurrentStateId))
            return (false, $"Action not allowed from current state {instance.CurrentStateId}.");

        instance.CurrentStateId = action.ToState;
        instance.History.Add(new WorkflowHistoryEntry { ActionId = action.Id });

        return (true, null);
    }

    public WorkflowInstance? GetInstance(string id)
    {
        return _instances.TryGetValue(id, out var instance) ? instance : null;
    }
}
