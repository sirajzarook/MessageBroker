using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Helpers
{
    public static class ArgsProcessor
    {
        public static bool TryGetApplicationMode(string[] args, out ApplicationMode result, out string  errorMessage)
        {
            int argsPosition = 0;
            if (args.Length < argsPosition+1)
            {
                result = ApplicationMode.UNKNOWN;
                errorMessage = $"Application mode in argument at position {argsPosition} is required";
                return false;
            }
            else
            {
                var value = args[0];
                result = value.ToApplicationMode();
                if(result == ApplicationMode.UNKNOWN)
                {
                    errorMessage = $"Invalid applcatiom mode argument at position {argsPosition}, value: {value} was not recognised ";
                    return false;
                }
                else
                {
                    errorMessage = String.Empty;
                    return true;
                }
            }
        }
    }
}
