using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace IngameScript
{
    partial class Program
    {
        public partial class FineRefineModule : IControllModule
        {
            public UpdateFrequency DistrubuteResources()
            {
                return _refineQueueSequence.StartSequence(DistributeSequence());
            }


            private IEnumerator<int> DistributeSequence()
            {
                while (true)
                {
                    //[TODO] Change filter
                    _program.GridTerminalSystem.GetBlocksOfType(_containers, p => p.InventoryCount==1 && !p.CustomName.Contains(_settings.RefinarySufix));
                    foreach (var refinary in _refinaries) {

                        var inentory = refinary.GetInventory(0);
                        //_logger.LogDebug($"Analazing refinary {refinary.CustomName}");
                        if (inentory.CurrentVolume * 4 < inentory.MaxVolume) {
                            //_logger.LogDebug($"Loading refinary {refinary.CustomName}");
                            var config = _refinaryConfigs[refinary];
                            
                            for (var i = 0; i < config.Priorities.Length; i++) {
                                var currentSearch = config.Priorities[i];
                                //_logger.LogDebug($"Looking for {currentSearch}");
                                foreach (var container in _containers)
                                {
                                    /*
                                    try
                                    {
                                        throw new NullReferenceException();
                                    }
                                    catch
                                    {
                                        _logger.LogInformation("Welcome to debug");
                                    }
                                    */
                                    //exclude production.
                                    var containerInventory = container.GetInventory(0);
                                    containerInventory.GetItems(_containerItems);
                                    for (var ii = 0; ii < _containerItems.Count; ii++)
                                    {
                                        var cntItem = _containerItems[ii];
                                        if (containerInventory.CanTransferItemTo(inentory, cntItem.Type)) {
                                            var pos = inentory.ItemCount + 1;
                                            containerInventory.TransferItemTo(inentory, ii, pos, false, 1);
                                            
                                        }
                                    }
                                    yield return 1;
                                }
                                yield return 1;
                            }
                        }
                        yield return 10;
                    }
                    yield return 1;
                }
            }
        }
    }
}
