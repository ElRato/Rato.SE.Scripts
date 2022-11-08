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

namespace IngameScript
{
    partial class Program
    {
        public class CargoQueue2
        {
            //public IMyTerminalBlock Owner { get; set; }
            //public IMyInventory Inventory { get; set; }
            //public CargoQueueItem[] Items;
            //private Dictionary<MyItemType, int> _index;
            //public bool WasFullfilledSinceReset { get; private set; }

            public CargoQueue2(int length)
            {
                /*
                Items = new CargoQueueItem[length];
                for (var i = 0; i < length; i++)
                {
                    Items[i] = new CargoQueueItem();
                }
                _index = new Dictionary<MyItemType, int>();
                */
            }
            /*
            public void Set(int position, MyItemType itemType, MyFixedPoint amount)
            {

                int existingPosition;
                if (_index.TryGetValue(itemType, out existingPosition))
                {
                    var existing = Items[existingPosition];
                    if (existing.Active)
                    {
                        existing.Active = false;
                    }
                }

                var overrided = Items[position];
                overrided.Active = true;
                overrided.ItemType = itemType;
                overrided.Fulfilled = false;
                overrided.Amount = amount;

                _index[itemType] = position;
            }

            public void ResetFullfilled()
            {
                foreach (var item in Items)
                {
                    item.Fulfilled = false;
                }
                WasFullfilledSinceReset = false;
            }

            public void Fullfill(MyItemType itemType, MyFixedPoint amount)
            {

                int existingPosition;
                if (_index.TryGetValue(itemType, out existingPosition))
                {
                    var existing = Items[existingPosition];
                    if (existing.Active)
                    {
                        WasFullfilledSinceReset = true;
                        existing.Amount -= amount;
                    }
                }

                throw new ArgumentException($"{itemType.TypeId} is not a part of this queue");
            }

            public MyFixedPoint HasInQueue(MyItemType itemType)
            {
                int existingPosition;
                if (_index.TryGetValue(itemType, out existingPosition))
                {
                    var existing = Items[existingPosition];
                    if (existing.Active)
                    {
                        return existing.Amount;
                    }
                }
                return 0;
            }
            */
        }
        /*
        public class CargoQueueItem
        {
            public bool Active;
            public bool Fulfilled;
            public MyItemType ItemType;
            public MyFixedPoint Amount;
        }
        */
    }
}
