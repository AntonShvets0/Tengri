using System;

namespace TengriLang.Language.System.Library
{
    public class TENGRI_math
    {
        public static int TENGRI_pow(dynamic[] args)
        {
            if (args.Length < 2 || !(args[1] is int) || !(args[0] is int)) return 0;
            return Math.Pow(args[0], args[1]);
        }

        public static double TENGRI_sqrt(dynamic[] args)
        {
            if (args.Length < 1 || !(args[0] is double)) return 0;
            return Math.Sqrt(args[0]);
        }

        public static double TENGRI_cos(dynamic[] args)
        {
            if (args.Length < 1 || !(args[0] is double)) return 0;
            return Math.Cos(args[0]);
        }
        
        public static double TENGRI_min(dynamic[] args)
        {
            if (args.Length == 1 && args[0] is TengriArray tengriArray)
            {
               
                
            } else if (args.Length == 2) return Math.Min(args[0], args[1]);

            return 0;
        }

        public static long TENGRI_factorial(dynamic[] args)
        {
            if (args.Length < 1 || !(args[0] is int)) return 0;

            var result = 1L;
            for (int i = 1; i < args[0]; i++)
            {
                result *= i;
            }
            
            return result;
        }
    }
}