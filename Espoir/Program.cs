using Espoir.Entities;
using Espoir.Scenarios;

var context = new EspoirContext();
Scenario startScenario = new Start(context);
var engine = new ScenarioEngine(context, startScenario);
engine.Run();