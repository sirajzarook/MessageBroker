using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor
{
    public enum ApplicationMode
    {
        UNKNOWN = 0,
        CHECK_IN = 1,
        UPDATE_ADDRESS = 2,
        SEND_ORDER = 3,

        TEST = 999,
    }

    public static class ApplicationModeExtensions
    {
        public static string ToName(this ApplicationMode @enum)
        {
            switch (@enum)
            {
                case ApplicationMode.CHECK_IN:
                    return "checkin";
                case ApplicationMode.SEND_ORDER:
                    return "sendorder";
                case ApplicationMode.UPDATE_ADDRESS:
                    return "updateaddress";
                case ApplicationMode.TEST:
                    return "test";
                default:
                    throw new NotImplementedException($"Enum name value not implemented. Enum {@enum} ");
            }
        }

        public static ApplicationMode ToApplicationMode(this string @from)
        {
            switch (@from)
            {
                case "checkin":
                    return ApplicationMode.CHECK_IN;
                case "sendorder":
                    return ApplicationMode.SEND_ORDER;
                case "updateaddress":
                    return ApplicationMode.UPDATE_ADDRESS;
                case "test":
                    return ApplicationMode.TEST;
                default:
                    return ApplicationMode.UNKNOWN;
            }
        }
    }

}
