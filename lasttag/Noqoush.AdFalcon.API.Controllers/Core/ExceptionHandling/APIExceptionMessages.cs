using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noqoush.AdFalcon.API.Controllers.Core.ExceptionHandling
{
    public class APIExceptionMessages
    {
        public const string ClientIdIsNotProvided = "ClientId parameter is missed";

        public const string ClientIdIsNotValid = "ClientId parameter isn't valid";

        public const string HashIsNotProvided = "Hash is missed";

        public const string HashIsTampered = "Hash value isn't valid";

        public const string MissingParameters = "Some parameters are missed";

        public const string VersionIsNotProvided = "Version parameter is missed";

        public const string VersionNotSupported = "Version isn't supported";

        public const string ExceedsQuota = "Your account has insufficient quota to proceed";

        public const string InvalidFromDateFormat  = "(fdate) parameter format is invalid";

        public const string InvalidToDateFormat = "(tdate) parameter format is invalid";

        public const string LengthExceedMaxNumber = "(l) parameter exceeds the maximum value ({0})";

        public const string InvalidGroupByOption = "(gb) parameter value is invalid ";

        public const string InvalidAppIdFormat = "(aid) parameter format is invalid";

        public const string InvalidCountryCode = "(cc) parameter is invalid";

        public const string InternalServerError = "Internal server error has occured, please try again or contact AdFalcon team for more help";

        public const string NowAllowedAccess = "Access is denied, please contact AdFalcon team for more help";

        public const string InvalidDateRange = "(fdate) parameter can't be greater than (tdate)";
    }
}
