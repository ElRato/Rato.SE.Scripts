using System;
using Malware.MDKUtilities;
using Sandbox.ModAPI.Ingame;
using SETestEnv;

namespace IngameScript.MDK
{
    public class Bootstrapper
    {
        // All the files in this folder, as well as all files containing the file ".debug.", will be excluded
        // from the build process. You can use this to create utilites for testing your scripts directly in 
        // Visual Studio.

        static Bootstrapper()
        {
            // Initialize the MDK utility framework
            MDKUtilityFramework.Load();
        }

        public static MyGridProgram PrepareFixture(MDKFactory.ProgramConfig config)
        {
            return MDKFactory.CreateProgram<Program>(config);
        }
        
        [STAThread]
        public static void Main()
        {
            var universe = UniversePreset.Minimalistic<Program>();
            universe.Start();
        }
    }
}