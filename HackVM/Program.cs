using System;
using System.Collections.Generic;
using System.IO;
using HackVM.Commands;

namespace HackVM
{
    class Program
    {
        private static Arguments _args;

        static void Main(string[] args)
        {
            _args = new Arguments(args);
            var listing = new VMCodeParser(_args.Path);

            var commands = new List<VMCommand>();

            foreach (var codeline in listing.CodeListing())
                commands.Add(new VMCommand(codeline, listing.FileName));

            OutputByteCode(commands);
        }

        private static void OutputByteCode(List<VMCommand> commands)
        {
            var directoryName = Path.GetDirectoryName(_args.Path);
            var listingFileName = Path.GetFileNameWithoutExtension(_args.Path);
            var outputFile = Path.Combine(directoryName, listingFileName) + ".asm";

            var translator = new CodeTranslator();
            if (File.Exists(outputFile))
                File.Delete(outputFile);
            foreach (var command in commands)
                File.AppendAllText(outputFile, translator.TranslateCommand(command));

            Console.WriteLine($"Saved asm code at: {outputFile}");
        }
    }
}