# StateMachine_API
This is a minimal backend service that implements a *configurable workflow engine* using *.NET 8 Minimal API*. It allows you to define workflows with states and actions, start workflow instances, transition between states, and query instance history — all backed by in-memory storage.

---

## Features

- Define custom workflows with named states and transitions
- Start instances based on a workflow definition
- Execute actions to move workflow instances between states
- Retrieve current state and execution history
- Validations for state/action rules
- Integrated Swagger UI for testing

---

## Tech Stack

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- ASP.NET Core Minimal API
- In-memory storage (no DB)
- Swagger / OpenAPI for API testing

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Run Locally

```bash
git clone https://github.com/RanveerRaj655/StateMachine_API.git
cd ./State_MachineAPI/
dotnet watch run
```

## Project Structure
```bash
StateMachineAPI/
├── Models/
│   ├── States.cs                # State model (Id, Name, IsInitial, IsFinal, Enabled)
│   ├── WorkflowActions.cs       # Transition action model
│   ├── WorkflowDefinitions.cs   # Contains a collection of States and Actions
│   └── WorkflowInstances.cs     # Running instance of a workflow with history
│
├── Services/
│   └── WorkflowServices.cs      # Core logic for managing definitions and instances
│
├── Program.cs                   # Minimal API endpoints and service wiring
├── assignment.csproj            # Project file
└── README.md  
``` 
