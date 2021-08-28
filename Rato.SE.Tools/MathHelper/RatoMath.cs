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
using System.Linq.Expressions;

namespace IngameScript
{
    partial class Program
    {
        class RatoMath
        {
            public float ShortestAnglePath(float currentAngle, float targetAnge)
            {
                var diff = targetAnge - currentAngle;
                if (diff > MathHelper.Pi)
                    diff -= MathHelper.TwoPi;
                else if (diff <= -MathHelper.Pi)
                    diff += MathHelper.TwoPi;
                return diff;
            }

            public bool GreaterThen(float comparisonValue, float baseValue, double accuracy) {
                var diff = comparisonValue - baseValue;
                return Math.Abs(diff) > accuracy ? (diff > 0 ? true: false) : false;
            }
            public bool Equal(float comparisonValue, float baseValue, double accuracy)
            {
                return Math.Abs(comparisonValue - baseValue) > accuracy ? false : true;
            }
        }
    }
}
