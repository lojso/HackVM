using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using HackVM.Commands;

namespace HackVM
{
    public class CodeTranslator
    {
        private readonly string _newLine = Environment.NewLine;

        public string TranslateCommand(VMCommand command)
        {
            if (command.CommandType == CommandType.PUSH)
                return TranslatePush(command);
            if (command.CommandType == CommandType.ARITHMETIC)
                return TranslateArithmetics(command);
            throw new InvalidEnumArgumentException($"Cant translate command {command}");
        }

        private string TranslatePush(VMCommand command)
        {
            return $"@{command.Arg2}{_newLine}" +
                   $"D=A{_newLine}" +
                   $"@SP{_newLine}" +
                   $"A=M{_newLine}" +
                   $"M=D{_newLine}" +
                   StackIncrement();
        }

        private string TranslateArithmetics(VMCommand command)
        {
            var operation = command.Command.Equals("add") ? "+" : "-";
            return StackDecrement() +
                   $"@SP{_newLine}" +
                   $"A=M{_newLine}" +
                   $"D=M{_newLine}" +
                   StackDecrement() +
                   $"A=M{_newLine}" +
                   $"M=D{operation}M{_newLine}" +
                   StackIncrement();
        }

        private string StackIncrement() =>
            $"@SP{_newLine}" +
            $"M=M+1{_newLine}";

        private string StackDecrement() =>
            $"@SP{_newLine}" +
            $"M=M-1{_newLine}";
    }
}