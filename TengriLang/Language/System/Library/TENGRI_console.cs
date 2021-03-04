using System;

namespace TengriLang.Language.System.Library
{
    public class TENGRI_console
    {
        public static dynamic TENGRI_print(dynamic[] TENGRI_SYS_ARGS)
        {
            Console.WriteLine(string.Join(", ", TENGRI_SYS_ARGS));

            return null;
        }

        public static dynamic TENGRI_input(dynamic[] TENGRI_SYS_ARGS)
        {
            return Console.ReadLine();
        }
    }
}