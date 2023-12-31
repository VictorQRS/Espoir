using Espoir.Entities;

namespace Espoir.Scenarios
{
    internal class ScenarioEngine
    {
        public ScenarioEngine(EspoirContext context, Scenario startingScenario)
        {
            this.Context = context;
            this.CurrentScenario = startingScenario;
        }

        public EspoirContext Context { get; }
        public Scenario CurrentScenario { get; private set; }

        public void Run()
        {
            while (true)
            {
                var scenario = this.CurrentScenario.Run();
                if (scenario == null)
                {
                    break;
                }

                this.CurrentScenario = scenario;
                Console.Clear();
            }
        }
    }
}
