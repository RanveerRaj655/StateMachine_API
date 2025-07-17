using State_MachineAPI.Services;
using State_MachineAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<WorkflowService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/definitions", (WorkflowDefinition def, WorkflowService service) =>
{
    var (success, error) = service.AddDefinition(def);
    return success ? Results.Ok("Definition created.") : Results.BadRequest(error);
});

app.MapGet("/definitions/{id}", (string id, WorkflowService service) =>
{
    var def = service.GetDefinition(id);
    return def is not null ? Results.Ok(def) : Results.NotFound("Definition not found.");
});

app.MapPost("/instances", (string definitionId, WorkflowService service) =>
{
    var (success, error, instance) = service.StartInstance(definitionId);
    return success ? Results.Ok(instance) : Results.BadRequest(error);
});

app.MapPost("/instances/{id}/execute", (string id, string actionId, WorkflowService service) =>
{
    var (success, error) = service.ExecuteAction(id, actionId);
    return success ? Results.Ok("Action executed.") : Results.BadRequest(error);
});

app.MapGet("/instances/{id}", (string id, WorkflowService service) =>
{
    var instance = service.GetInstance(id);
    return instance is not null ? Results.Ok(instance) : Results.NotFound("Instance not found.");
});

app.Run();
