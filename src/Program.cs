using System.Diagnostics.CodeAnalysis;

namespace ARM9Editor
{
    internal static class Program
    {
        [STAThread]
        [RequiresAssemblyFiles("Calls ARM9Editor.App.App()")]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}