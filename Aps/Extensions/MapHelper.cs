namespace Aps.Extensions
{
    public class MapHelper
    {
        public static T MapEnum<T>(string value)
        {
            return EnumHelper<T>.Parse(value);
        }
    }
}