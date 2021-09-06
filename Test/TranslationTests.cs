using System;
using HackVM;
using HackVM.Commands;
using NUnit.Framework;

namespace Test
{
    public class TranslationTests
    {
        private readonly string _newLine = Environment.NewLine;
        private CodeTranslator _translator;

        [SetUp]
        public void Setup()
        {
            _translator = new CodeTranslator();
        }

        #region Memory

        [Test]
        public void PushConst()
        {
            var vmCommand = new VMCommand("push constant 7", "Tests");

            var result = _translator.TranslateCommand(vmCommand);

            Assert.True(result.Equals($"@7{_newLine}" +
                                      $"D=A{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"A=M{_newLine}" +
                                      $"M=D{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"M=M+1{_newLine}"));
        }

        [Test]
        public void PushStatic()
        {
            var vmCommand = new VMCommand("push static 7", "Tests");

            var result = _translator.TranslateCommand(vmCommand);

            Assert.True(result.Equals($"@Tests.7{_newLine}" +
                                      $"D=M{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"A=M{_newLine}" +
                                      $"M=D{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"M=M+1{_newLine}"));
        }

        [Test]
        public void PushLocal()
        {
            var vmCommand = new VMCommand("push local 7", "Tests");

            var result = _translator.TranslateCommand(vmCommand);

            Assert.True(result.Equals($"@7{_newLine}" +
                                      $"D=A{_newLine}" +
                                      $"@LCL{_newLine}" +
                                      $"A=A+D{_newLine}" +
                                      $"D=M{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"A=M{_newLine}" +
                                      $"M=D{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"M=M+1{_newLine}"));
        }

        [Test]
        public void PopLocal()
        {
            var vmCommand = new VMCommand("pop local 2", "Tests");

            var result = _translator.TranslateCommand(vmCommand);

            Assert.True(result.Equals($"@2{_newLine}" +
                                      $"D=A{_newLine}" +
                                      $"@LCL{_newLine}" +
                                      $"A=A+D{_newLine}" +
                                      $"D=A{_newLine}" +
                                      $"@R13{_newLine}" +
                                      $"M=D{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"M=M-1{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"A=M{_newLine}" +
                                      $"D=M{_newLine}" +
                                      $"@R13{_newLine}" +
                                      $"A=M{_newLine}" +
                                      $"M=D{_newLine}"));
        }

        [Test]
        public void PopStatic()
        {
            var vmCommand = new VMCommand("pop static 2", "Tests");

            var result = _translator.TranslateCommand(vmCommand);

            Assert.True(result.Equals($"@SP{_newLine}" +
                                      $"M=M-1{_newLine}" +
                                      $"@SP{_newLine}" +
                                      $"A=M{_newLine}" +
                                      $"D=M{_newLine}" +
                                      $"@Tests.2{_newLine}" +
                                      $"M=D{_newLine}"));
        }

        #endregion

        #region Arithmetic

        [Test]
        public void Add()
        {
            var vmCommand = new VMCommand("add", "Tests");

            var result = _translator.TranslateCommand(vmCommand);

            Assert.True(result.Equals($"@SP{_newLine}" +
                                      $"AM=M-1{_newLine}" +
                                      $"D=M{_newLine}" +
                                      $"A=A-1{_newLine}" +
                                      $"M=M+D{_newLine}"));
        }

        [Test]
        public void Sub()
        {
            var vmCommand = new VMCommand("sub", "Tests");

            var result = _translator.TranslateCommand(vmCommand);

            Assert.True(result.Equals($"@SP{_newLine}" +
                                      $"AM=M-1{_newLine}" +
                                      $"D=M{_newLine}" +
                                      $"A=A-1{_newLine}" +
                                      $"M=M-D{_newLine}"));
        }

        #endregion
    }
}