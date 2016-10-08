namespace AWeb
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Removes dashes ("-") from the given object value represented as a string and returns an empty string ("")
        ///     when the instance type could not be represented as a string.
        ///     <para>
        ///         Note: This will return the type name of given isntance if the runtime type of the given isntance is not a
        ///         string!
        ///     </para>
        /// </summary>
        /// <param name="value">The object instance to undash when represented as its string value.</param>
        /// <returns></returns>
        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        public static string AddBlank(this string value, int num)
        {
            switch (num)
            {
                case 0:
                    return string.Concat("", value);
                case 1:
                    return string.Concat("©¸", value);
                case 2:
                    return string.Concat("¡¡©¸", value);
                case 3:
                    return string.Concat("¡¡¡¡©¸", value);
                default:
                    return string.Concat("", value);
            }


        }

        /// <summary>
        ///     Removes dashes ("-") from the given string value.
        /// </summary>
        /// <param name="value">The string value that optionally contains dashes.</param>
        /// <returns></returns>
        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }
    }
}