using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HackVM
{
    class Program
    {
        private static Arguments _args;
        
        static void Main(string[] args)
        {
            _args = new Arguments(args);
        }
    }

    public class VMCodeParser
    {
        private string _filePath;
        
        public VMCodeParser(string filePath)
        {
            _filePath = filePath;
        }
        
        public IEnumerable<string> CodeListing()
        {
            using var file = new StreamReader(_filePath);
            string line; 
            while ((line = file.ReadLine()) != null)
            {
                var parsedLine = RemoveWhitespace(line);
                parsedLine = RemoveComments(parsedLine);
                if(parsedLine.Length != 0)
                    yield return parsedLine;
            }
        }
        
        
        private string RemoveWhitespace(string input)
        {
            return new string(input
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        private string RemoveComments(string line)
        {
            var commentSymbol = "//";
            var firstOccurence = line.IndexOf(commentSymbol, StringComparison.Ordinal);
            if (firstOccurence == -1)
                return line;
            return line.Substring(0, firstOccurence);
        }
    }
}