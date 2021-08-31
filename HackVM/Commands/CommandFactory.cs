using System.Runtime.InteropServices;

namespace HackVM.Commands
{
    public class CommandFactory
    {
        private readonly CodeTranslator _translator;

        public CommandFactory()
        {
            _translator = new CodeTranslator();
        }
    }
}