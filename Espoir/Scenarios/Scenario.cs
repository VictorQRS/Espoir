using Espoir.Entities;

namespace Espoir.Scenarios
{
    internal abstract class Scenario
    {
        public EspoirContext Context { get; set; }

        public Scenario(EspoirContext context)
        {
            this.Context = context;
        }

        public abstract Scenario? Run();
    }
}