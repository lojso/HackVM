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
            if (command.CommandType == CommandType.POP)
                return TranslatePop(command);
            if (command.CommandType == CommandType.ARITHMETIC)
                return TranslateArithmetics(command);
            throw new InvalidEnumArgumentException($"Cant translate command {command}");
        }

        private string TranslatePush(VMCommand command)
        {
            if (command.Arg1.Equals("local"))
                return LocalPush(command);
            if (command.Arg1.Equals("constant"))
                return ConstantPush(command);
            if (command.Arg1.Equals("static"))
                return StaticPush(command);
            return "";
        }

        private string TranslatePop(VMCommand command)
        {
            if (command.Arg1.Equals("local"))
                return LocalPop(command);
            if (command.Arg1.Equals("static"))
                return StaticPop(command);
            return "";
        }

        private string LocalPop(VMCommand command)
        {
            return $"@2{_newLine}" +
                   $"D=A{_newLine}" +
                   $"@LCL{_newLine}" +
                   $"A=A+D{_newLine}" +
                   $"D=A{_newLine}" +
                   $"@R13{_newLine}" +
                   $"M=D{_newLine}" +
                   StackDecrement() +
                   $"@SP{_newLine}" +
                   $"A=M{_newLine}" +
                   $"D=M{_newLine}" +
                   $"@R13{_newLine}" +
                   $"A=M{_newLine}" +
                   $"M=D{_newLine}";
        }

        private string StaticPop(VMCommand command)
        {
            return $"@SP{_newLine}" +
                   $"M=M-1{_newLine}" +
                   $"@SP{_newLine}" +
                   $"A=M{_newLine}" +
                   $"D=M{_newLine}" +
                   $"@{command.FileName}.{command.Arg2}{_newLine}" +
                   $"M=D{_newLine}";
        }

        private string LocalPush(VMCommand command)
        {
            return $"@{command.Arg2}{_newLine}" +
                   $"D=A{_newLine}" +
                   $"@LCL{_newLine}" +
                   $"A=A+D{_newLine}" +
                   $"D=M{_newLine}" +
                   $"@SP{_newLine}" +
                   $"A=M{_newLine}" +
                   $"M=D{_newLine}" +
                   StackIncrement();
        }

        private string StaticPush(VMCommand command)
        {
            return $"@{command.FileName}.{command.Arg2}{_newLine}" +
                   $"D=M{_newLine}" +
                   $"@SP{_newLine}" +
                   $"A=M{_newLine}" +
                   $"M=D{_newLine}" +
                   $"@SP{_newLine}" +
                   $"M=M+1{_newLine}";
        }

        private string ConstantPush(VMCommand command)
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
            return $"@SP{_newLine}" +
                   $"AM=M-1{_newLine}" +
                   $"D=M{_newLine}" +
                   $"A=A-1{_newLine}" +
                   $"M=M{operation}D{_newLine}";
        }

        private string StackIncrement() =>
            $"@SP{_newLine}" +
            $"M=M+1{_newLine}";

        private string StackDecrement() =>
            $"@SP{_newLine}" +
            $"M=M-1{_newLine}";
    }
}