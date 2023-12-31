using Espoir.Entities;
using Espoir.Scenarios;

var context = new EspoirContext();
Scenario startScenario = new Start(context);
var engine = new ScenarioEngine(context, startScenario);
engine.Run();

// TODO: Hall Scenario (player list, card frequency)
// TODO: Player Communication Scenario (start battle)
// TODO: Battle Scenario 
