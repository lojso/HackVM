using HackVM.Commands;
using NUnit.Framework;

namespace Test
{
    public class CommandTests
    {
        [Test]
        public void PopConst()
        {
            // TODO
        }

        [TestCase("push local 7", "push", CommandType.PUSH, "local", "7")]
        [TestCase("push static 7", "push", CommandType.PUSH, "static", "7")]
        [TestCase("push constant 7", "push", CommandType.PUSH, "constant", "7")]
        [TestCase("push this 7", "push", CommandType.PUSH, "this", "7")]
        [TestCase("push that 7", "push", CommandType.PUSH, "that", "7")]
        [TestCase("push pointer 7", "push", CommandType.PUSH, "pointer", "7")]
        [TestCase("push temp 7", "push", CommandType.PUSH, "temp", "7")]
        [TestCase("push argument 7", "push", CommandType.PUSH, "argument", "7")]
        public void PushTest(
            string commandLine,
            string expCommand,
            CommandType expCommandType,
            string source,
            string number)
        {
            var vmCommand = new VMCommand(commandLine, "Tests");

            Assert.True(vmCommand.Command.Equals(expCommand));
            Assert.True(vmCommand.CommandType == expCommandType);
            Assert.True(vmCommand.Arg1.Equals(source));
            Assert.True(vmCommand.Arg2.Equals(number));
            Assert.True(vmCommand.FileName.Equals("Tests"));
        }

        [TestCase("pop local 7", "pop", CommandType.POP, "local", "7")]
        [TestCase("pop static 7", "pop", CommandType.POP, "static", "7")]
        [TestCase("pop this 7", "pop", CommandType.POP, "this", "7")]
        [TestCase("pop that 7", "pop", CommandType.POP, "that", "7")]
        [TestCase("pop pointer 7", "pop", CommandType.POP, "pointer", "7")]
        [TestCase("pop temp 7", "pop", CommandType.POP, "temp", "7")]
        [TestCase("pop argument 7", "pop", CommandType.POP, "argument", "7")]
        public void PopTest(
            string commandLine,
            string expCommand,
            CommandType expCommandType,
            string source,
            string number)
        {
            var vmCommand = new VMCommand(commandLine, "Tests");

            Assert.True(vmCommand.Command.Equals(expCommand));
            Assert.True(vmCommand.CommandType == expCommandType);
            Assert.True(vmCommand.Arg1.Equals(source));
            Assert.True(vmCommand.Arg2.Equals(number));
            Assert.True(vmCommand.FileName.Equals("Tests"));
        }
    }
}