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
        public partial class CargoControlModule : IControllModule
        {
            private const int _yellowValue = 70;
            private const int _redValue = 95;

            public UpdateFrequency CapacityDisplay()
            {
                return _capacityDisplayExecutor.StartSequence(CapacityDisplaySequence());
            }

            private IEnumerator<int> CapacityDisplaySequence()
            {
                while (true) {
                    /*
                    var availableTexSize = _capacityLcd.SurfaceSize * (1 - 2 * (_capacityLcd.TextPadding / 100f));
                    var offset = (_capacityLcd.TextureSize - availableTexSize) / 2;
                    */

                    MyFixedPoint totalCapacity = 0;
                    MyFixedPoint totalUsed = 0;
                    for (var i = 0; i < _cargoContainers.Count; i++)
                    {
                        for (var ix = 0; ix < _cargoContainers[i].InventoryCount; ix++)
                        {
                            var inv = _cargoContainers[i].GetInventory(ix);
                            totalCapacity += inv.MaxVolume;
                            totalUsed += inv.CurrentVolume;
                        }
                    }

                    _logger.LogInformation($"Calculated cargo used {totalUsed}");

                    int percent = (int)Math.Round(((float)totalCapacity)== 0? 100: ((float)totalUsed) / (float)totalCapacity *100);
                    /*
                    _logger.LogInformation($"Calculated cargo percent {percent}");
                    _logger.LogInformation($"SurfaceSize {_capacityLcd.SurfaceSize}");
                    _logger.LogInformation($"TextureSize {_capacityLcd.TextureSize}");
                    _logger.LogInformation($"Viewport {_viewport}");
                    */
                    var frame = _capacityLcd.DrawFrame();
                    DrawSprites(ref frame, percent);
                    frame.Dispose();
                    yield return 100;
                }
            }

            public void DrawSprites(ref MySpriteDrawFrame frame, int percent)
            {
                var color = Color.Green;
                if (percent > _yellowValue)
                    color = Color.Yellow;
                if (percent > _redValue)
                    color = Color.Red;


                var windowSize = _viewport.Size - 10;


                _logger.LogInformation($"windowSize {windowSize}");

                var barSize = new Vector2((windowSize.X) / 100 * percent, windowSize.Y / 2);

                _logger.LogInformation($"barSize {barSize}");

                if (barSize.X != 0)
                {
                    var barPosition = new Vector2(5, 5) + new Vector2(0, barSize.Y / 2 * 3) + _viewport.Position;

                    _logger.LogInformation($"barPosition {barPosition}");
                    var sprite = new MySprite()
                    {
                        Type = SpriteType.TEXTURE,
                        Data = "SquareSimple",
                        Position = barPosition,
                        Size = barSize,
                        Color = color.Alpha(1f),
                        Alignment = TextAlignment.LEFT
                    };
                    frame.Add(sprite);
                }

                var textSize = new Vector2(windowSize.X, windowSize.Y / 2);

                _logger.LogInformation($"textSize {textSize}");

                var textPosition = new Vector2(5, 5) + new Vector2(textSize.X / 2, textSize.Y / 4) + _viewport.Position;
                _logger.LogInformation($"textPosition {textPosition}");

                var textsprite = new MySprite()
                {
                    Type = SpriteType.TEXT,
                    Data = "Cargo",
                    Position = textPosition,
                    Size = textSize,
                    Color = color.Alpha(1),
                    RotationOrScale = 1.5f /* 80 % of the font's default size */,
                    Alignment = TextAlignment.CENTER /* Center the text on the position */,
                    FontId = "White"
                };
                frame.Add(textsprite);
            }
        }
    }
}
