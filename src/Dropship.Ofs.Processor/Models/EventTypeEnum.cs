using System;
using System.Collections.Generic;
using System.Text;

namespace Dropship.Ofs.Processor.Models
{
    public enum EventTypeEnum
    {
        UNKNOWN =0,
        RECEIVED = 1,
        SENT = 2,
    }

    public static class EventTypeEnumExtensions
    {
        public static string ToName(this EventTypeEnum @enum)
        {
            switch (@enum)
            {
                case EventTypeEnum.RECEIVED:
                    return "RECEIVED";
                case EventTypeEnum.SENT:
                    return "SENT";
                default:
                    throw new NotImplementedException($"Enum name value not implemented. Enum {@enum} ");
            }
        }
    }
    }
