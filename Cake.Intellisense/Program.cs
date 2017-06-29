using Cake.Intellisense.Core;
using System.Diagnostics;
using System.IO;

namespace Cake.Intellisense
{
    class Program
    {
        static void Main(string[] args)
        {
            string dllsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "cake-dlls");

            // ensures all of the cake dll's are accessible
            Process.Start("dotnet", $"publish -o \"{dllsDirectory}\"").WaitForExit();

            new CakeFileIntellisenseGenerator().GenereteIntellisense(dllsDirectory);

            new CakeFileConverter().ConvertCsToCake();
        }
    }
}
