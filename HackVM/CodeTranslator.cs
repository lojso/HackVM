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
            if (command.Arg1.Equals("this"))
                return ThisPush(command);
            if (command.Arg1.Equals("that"))
                return ThatPush(command);
            if (command.Arg1.Equals("argument"))
                return ArgumentPush(command);
            if (command.Arg1.Equals("temp"))
                return TempPush(command);
            if (command.Arg1.Equals("pointer"))
                return PointerPush(command);
            if (command.Arg1.Equals("constant"))
                return ConstantPush(command);
            if (command.Arg1.Equals("static"))
                return StaticPush(command);
            throw new InvalidEnumArgumentException($"Cant translate command {command}");
        }

        private string TranslatePop(VMCommand command)
        {
            if (command.Arg1.Equals("local"))
                return LocalPop(command);
            if (command.Arg1.Equals("this"))
                return ThisPop(command);
            if (command.Arg1.Equals("that"))
                return ThatPop(command);
            if (command.Arg1.Equals("argument"))
                return ArgumentPop(command);
            if (command.Arg1.Equals("temp"))
                return TempPop(command);
            if (command.Arg1.Equals("static"))
                return StaticPop(command);
            if (command.Arg1.Equals("pointer"))
                return PointerPop(command);
            return "";
        }

        private string PointerPop(VMCommand command)
        {
            var baseAddress = command.Arg2.Equals("0") ? "@THIS" : "@THAT";
            return $"{baseAddress}{_newLine}" +
                   $"D=A{_newLine}" +
                   $"@R13{_newLine}" +
                   $"M=D{_newLine}" +
                   $"@SP{_newLine}" +
                   $"AM=M-1{_newLine}" +
                   $"D=M{_newLine}" +
                   $"@R13{_newLine}" +
                   $"A=M{_newLine}" +
                   $"M=D{_newLine}";
        }

        private string LocalPop(VMCommand command) =>
            SegmentPop(command, "@LCL");

        private string ThisPop(VMCommand command) =>
            SegmentPop(command, "@THIS");

        private string ThatPop(VMCommand command) =>
            SegmentPop(command, "@THAT");

        private string ArgumentPop(VMCommand command) =>
            SegmentPop(command, "@ARG");

        private string TempPop(VMCommand command) =>
            $"@{command.Arg2}{_newLine}" +
            $"D=A{_newLine}" +
            $"@R5{_newLine}" +
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

        private string SegmentPop(VMCommand command, string segmentBase) =>
            $"@{command.Arg2}{_newLine}" +
            $"D=A{_newLine}" +
            $"{segmentBase}{_newLine}" +
            $"A=M+D{_newLine}" +
            $"D=A{_newLine}" +
            $"@R13{_newLine}" +
            $"M=D{_newLine}" +
            $"@SP{_newLine}" +
            $"AM=M-1{_newLine}" +
            $"D=M{_newLine}" +
            $"@R13{_newLine}" +
            $"A=M{_newLine}" +
            $"M=D{_newLine}";

        private string StaticPop(VMCommand command) =>
            $"@SP{_newLine}" +
            $"M=M-1{_newLine}" +
            $"@SP{_newLine}" +
            $"A=M{_newLine}" +
            $"D=M{_newLine}" +
            $"@{command.FileName}.{command.Arg2}{_newLine}" +
            $"M=D{_newLine}";

        private string LocalPush(VMCommand command) =>
            PushFromSegment(command, "@LCL");

        private string ArgumentPush(VMCommand command) =>
            PushFromSegment(command, "@ARG");

        private string ThatPush(VMCommand command) =>
            PushFromSegment(command, "@THAT");

        private string ThisPush(VMCommand command) =>
            PushFromSegment(command, "@THIS");

        private string TempPush(VMCommand command) =>
            $"@{command.Arg2}{_newLine}" +
            $"D=A{_newLine}" +
            $"@R5{_newLine}" +
            $"A=A+D{_newLine}" +
            $"D=M{_newLine}" +
            $"@SP{_newLine}" +
            $"A=M{_newLine}" +
            $"M=D{_newLine}" +
            StackIncrement();

        private string PointerPush(VMCommand command)
        {
            var baseAddress = command.Arg2.Equals("0") ? "@THIS" : "@THAT";
            return $"{baseAddress}{_newLine}" +
                   $"D=M{_newLine}" +
                   $"@SP{_newLine}" +
                   $"A=M{_newLine}" +
                   $"M=D{_newLine}" +
                   $"@SP{_newLine}" +
                   $"M=M+1{_newLine}";
        }

        private string PushFromSegment(VMCommand command, string segmentBase) =>
            $"@{command.Arg2}{_newLine}" +
            $"D=A{_newLine}" +
            $"{segmentBase}{_newLine}" +
            $"A=M+D{_newLine}" +
            $"D=M{_newLine}" +
            $"@SP{_newLine}" +
            $"A=M{_newLine}" +
            $"M=D{_newLine}" +
            StackIncrement();

        private string StaticPush(VMCommand command) =>
            $"@{command.FileName}.{command.Arg2}{_newLine}" +
            $"D=M{_newLine}" +
            $"@SP{_newLine}" +
            $"A=M{_newLine}" +
            $"M=D{_newLine}" +
            $"@SP{_newLine}" +
            $"M=M+1{_newLine}";

        private string ConstantPush(VMCommand command) =>
            $"@{command.Arg2}{_newLine}" +
            $"D=A{_newLine}" +
            $"@SP{_newLine}" +
            $"A=M{_newLine}" +
            $"M=D{_newLine}" +
            StackIncrement();

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