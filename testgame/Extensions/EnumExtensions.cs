using System;
using System.Collections.Generic;
using System.Text;

namespace Pong
{
    public static class EnumExtensions
    {
        public static String AsString(this Enum thisEnum)
        {
            return Enum.GetName(thisEnum.GetType(), thisEnum);
        }

    }
}
