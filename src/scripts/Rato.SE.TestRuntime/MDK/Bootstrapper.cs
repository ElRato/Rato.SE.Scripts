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
            // In order for your program to actually run, you will need to provide a mockup of all the facilities 
            // your script uses from the game, since they're not available outside of the game.

            // Create and configure the desired program.
            var universe = Universes.TestUniverse<Program>();
            universe.Start();

            //var program = MDKFactory.CreateProgram<Program>();
            //MDKFactory.Run(program);
        }
    }
}