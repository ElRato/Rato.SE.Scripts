using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace IngameScript
{
    public class DumbModule : Program.IControllModule
    {
        public DumbModule()
        {
            StatusDetails = new List<Program.ModuleStatusDetail>();
        }

        public void Initialize()
        {
            Status = Program.ModuleStatus.Initialized;
            StatusDetails.Add(new Program.ModuleStatusDetail()
            {
                Level = Program.ActionStatus.Ok,
                Name = "DumbModule"
            });
        }

        public Program.ModuleStatus Status { get; set; }
        public List<Program.ModuleStatusDetail> StatusDetails { get; }
        public UpdateFrequency ContinueSequence(UpdateType updateSource)
        {
            return UpdateFrequency.Update1;
        }
    }
}