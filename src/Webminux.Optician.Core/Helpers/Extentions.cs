using Abp.UI;
using System;

namespace Webminux.Optician.Core.Helpers
{
    public static class Extentions
    {
        // take generic enum and return its value as string
        public static string GetEnumValueAsString(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public static DateTime ConvertDateStringToDate(this string value)
        {
            try
            {
                return DateTime.ParseExact(value, OpticianConsts.DateFormate, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Provided date is invalid");
            }
        }

        public static DateTime ConvertDateTimeStringToDateTime(this string value)
        {
            try
            {
                return DateTime.ParseExact(value, OpticianConsts.DateTimeFormate, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Provided date is invalid");
            }
        }
    }
}