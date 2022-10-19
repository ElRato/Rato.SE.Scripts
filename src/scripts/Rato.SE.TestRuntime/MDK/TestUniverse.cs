using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv
{
    public static partial class Universes
    {
        public static Universe TestUniverse<TProgram>() where TProgram : MyGridProgram
        {
            var universe = new Universe();

            var cubeGrid = new TestCubeGrid();
            var pb = new TestProgrammableBlock()
            {
                CustomName = "TestPB",
                CustomData = "",
                Program = new DeferredProgram<TProgram>(),
            };
            cubeGrid.AddBlock(pb);

            var textPanel = new TestTextPanel
            {
                CustomName = "[ccd] TestLCD",
                DetailedInfo = "Type: LCD Panel\nMax Required Input: 100 W\nCurrent Input: 100 W",

                Title = "Working;Time Global Time: ;Cargo;Power;Inventory;Echo;Center << Damage >>;Damage;BlockCount",

                BuildIntegrity = 7200,
                MaxIntegrity = 7200,
                
            };
            cubeGrid.AddBlock(textPanel);

            var battery = new TestBatteryBlock
            {
                CustomName = "Battery",
                DetailedInfo =
                    "Type: Battery\n" +
                    "Max Output: 12.00 MW\n" +
                    "Max Required Input: 12.00 MW\n" +
                    "Max Stored Power: 3.00 MWh\n" +
                    "Current Input: 0 W\n" +
                    "Current Output: 1.43 MWh\n" +
                    "Stored power: 2.47 MWh\n" +
                    "Fully depleted in: 1 days",

                BuildIntegrity = 7200,
                MaxIntegrity = 7200,
                CurrentDamage = 200
            };
            cubeGrid.AddBlock(battery);

            universe.AddCubeGrid(cubeGrid);

            return universe;
        }

    }
}
