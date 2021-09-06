namespace HackVM.Commands
{
    public class VMCommand
    {
        public VMCommand(string codeLine, string fileName)
        {
            var lineParts = codeLine.Split(' ');
            CommandType = ParseType(lineParts);
            Command = ParseCommand(lineParts);
            Arg1 = ParseFirstArg(lineParts);
            Arg2 = ParseSecondArg(lineParts);

            FileName = fileName;
        }

        public CommandType CommandType { get; }
        public string Command { get; }
        public string Arg1 { get; }
        public string Arg2 { get; }
        public string FileName { get; }


        private CommandType ParseType(string[] codeLine)
        {
            if (codeLine[0].StartsWith("pop"))
                return CommandType.POP;
            if (codeLine[0].StartsWith("push"))
                return CommandType.PUSH;
            return CommandType.ARITHMETIC;
        }

        private string ParseCommand(string[] codeLine) =>
            codeLine?[0];

        private string ParseFirstArg(string[] codeLine) =>
            codeLine?.Length > 1 ? codeLine[1] : null;

        private string ParseSecondArg(string[] codeLine) =>
            codeLine?.Length > 2 ? codeLine[2] : null;
    }

    public enum CommandType
    {
        ARITHMETIC,
        PUSH,
        POP,
        LABEL,
        GOTO,
        IF,
        FUNCTION,
        RETURN,
        CALL,
        UNDEFINED
    }

    public enum StackType
    {
        Argument,
        Local,
        Static,
        Constant,
        This,
        That,
        Pointer,
        Temp,
        Undefined
    }
}