// <copyright>Copyright (c) 2014 SpryMedia Ltd - All Rights Reserved</copyright>
//
// <summary>
// Validation methods for Editor
// </summary>
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DataTables.EditorUtil;

namespace DataTables
{
    /// <summary>
    /// Validation methods for DataTables Editor fields. All of the methods
    /// defined in this class return a delegate that can be used by
    /// <code>Field</code> instance's <code>Validator</code> method.
    ///
    /// Each method may define its own parameters that configure how the
    /// formatter operates. For example the `minLen` validator takes information
    /// on the minimum length of value to accept.
    ///
    /// Additionally each method can optionally take a <code>ValidationOpts</code>
    /// instance that controls common validation options and error messages.
    ///
    /// The validation delegates return `null` for validate data and a string for
    /// invalid data, with the string being the error message.
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// No validation - all inputs are valid.
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> None(ValidationOpts cfg = null)
        {
            return (val, data, host) => null;
        }

        /// <summary>
        /// Basic validation - this is used to perform the validation provided by the
        /// validation options only. If the validation options pass (e.g. `required`,
        /// `empty` and `optional`) then the validation will pass regardless of the
        /// actual value.
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Basic(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                return common == false ? opts.Message : null;
            };
        }

        /// <summary>
        /// Required field - there must be a value and it must be a non-empty value
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Required(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);
            opts.Empty = false;
            opts.Optional = false;

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                return common == false ? opts.Message : null;
            };
        }

        /// <summary>
        /// Optional field, but if given there must be a non-empty value
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> NotEmpty(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);
            opts.Empty = false;

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                return common == false ? opts.Message : null;
            };
        }


        /// <summary>
        /// Validate an input as a boolean value.
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Boolean(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);
            opts.Empty = false;

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                try
                {
                    Convert.ToBoolean(val);
                    return null;
                }
                catch (Exception) { }

                return opts.Message;
            };
        }


        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Number validation methods
         */

        /// <summary>
        /// Check that any input is numeric.
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Numeric(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                try
                {
                    Convert.ToDecimal(val);
                    return null;
                }
                catch (Exception) { }

                return opts.Message;
            };
        }

        /// <summary>
        /// Check for a numeric input and that it is greater than a given value.
        /// </summary>
        /// <param name="min">Minimum value the numeric value can take</param>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> MinNum(Decimal min, ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                var numeric = Numeric(opts)(val, data, host);
                if (numeric != null)
                {
                    return numeric;
                }

                // Converted to dec must be possible since Numeric passed to get here
                return Convert.ToDecimal(val) < min ?
                    opts.Message :
                    null;
            };
        }

        /// <summary>
        /// Check for a numeric input and that it is less than a given value.
        /// </summary>
        /// <param name="max">Maximum value the numeric value can take</param>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> MaxNum(Decimal max, ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                var numeric = Numeric(opts)(val, data, host);
                if (numeric != null)
                {
                    return numeric;
                }

                // Converted to dec must be possible since Numeric passed to get here
                return Convert.ToDecimal(val) > max ?
                    opts.Message :
                    null;
            };
        }

        /// <summary>
        /// Check for a numeric input and that it is both less than and greater than given values
        /// </summary>
        /// <param name="min">Minimum value the numeric value can take</param>
        /// <param name="max">Maximum value the numeric value can take</param>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> MinMaxNum(Decimal min, Decimal max, ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                var numeric = Numeric(opts)(val, data, host);
                if (numeric != null)
                {
                    return numeric;
                }

                // Converted to dec must be possible since Numeric passed to get here
                var dec = Convert.ToDecimal(val);
                return dec < min || dec > max ?
                    opts.Message :
                    null;
            };
        }



        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * String validation methods
         */

        /// <summary>
        /// Validate an input as an e-mail address.
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Email(ValidationOpts cfg = null)
        {
            ValidationOpts opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                try
                {
                    var str = Convert.ToString(val);
                    new System.Net.Mail.MailAddress(str);
                    return null; // If no exceptions - then valid
                }
                catch (Exception) { }

                return opts.Message;
            };
        }

        /// <summary>
        /// Validate a string has a minimum length.
        /// </summary>
        /// <param name="min">Minimum length</param>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> MinLen(int min, ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                var str = Convert.ToString(val);
                return str.Length < min ?
                    opts.Message :
                    null;
            };
        }

        /// <summary>
        /// Validate a string does not exceed a maximum length.
        /// </summary>
        /// <param name="max">Maximum length</param>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> MaxLen(int max, ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                var str = Convert.ToString(val);
                return str.Length > max ?
                    opts.Message :
                    null;
            };
        }

        /// <summary>
        /// Require a string with a certain minimum or maximum number of characters.
        /// </summary>
        /// <param name="min">Minimum length</param>
        /// <param name="max">Maximum length</param>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> MinMaxLen(int min, int max, ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                var str = Convert.ToString(val);
                return str.Length < min || str.Length > max ?
                    opts.Message :
                    null;
            };
        }

        /// <summary>
        /// Validate as an IP address.
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Ip(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                var str = Convert.ToString(val);
                System.Net.IPAddress addr;
                return System.Net.IPAddress.TryParse(str, out addr) ?
                    null :
                    opts.Message;
            };
        }

        /// <summary>
        /// Validate as an URL address.
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Url(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                var str = Convert.ToString(val);
                Uri addr;
                return Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out addr) ?
                    null :
                    opts.Message;
            };
        }


        /// <summary>
        /// Check if string could contain an XSS attack string
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Xss(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                if (!(val is string))
                {
                    return null;
                }

                return host.Field.XssSafety((string)val) == (string)val
                    ? null
                    : opts.Message;
            };
        }


        /// <summary>
        /// Ensure that the submitted string does not contain HTML tags
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> NoTags(ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                if (!(val is string))
                {
                    return null;
                }

                var tagRegex = new Regex(@"<[^>]+>");

                return tagRegex.IsMatch((string)val)
                    ? opts.Message
                    : null;
            };
        }



        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Date validation methods
         */

        /// <summary>
        /// Check that a valid date input is given
        /// </summary>
        /// <param name="format">Format that the date / time should be given in</param>
        /// <param name="cfg">Validation options</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> DateFormat(string format, ValidationOpts cfg = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                try
                {
                    var str = Convert.ToString(val);
                    DateTime.ParseExact(str, format, System.Globalization.CultureInfo.InvariantCulture);
                    return null; // If no exceptions - then valid
                }
                catch (Exception) { }

                return opts.Message;
            };
        }



        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Database validation methods
         */

        /// <summary>
        /// Check that the given value is unique in the database
        /// </summary>
        /// <param name="cfg">Validation options</param>
        /// <param name="column">Column name to use to check as a unique value. If not given the host field's database column name is used</param>
        /// <param name="table">Table to check that this value is uniquely valid on. If not given the host Editor's table name is used</param>
        /// <param name="db">Database connection. If not given the host Editor's database connection is used</param>
        /// <returns>Validation delegate</returns>
        public static Func<object, Dictionary<string, object>, ValidationHost, string> Unique(ValidationOpts cfg = null, string column = null, string table = null, Database db = null)
        {
            var opts = ValidationOpts.Select(cfg);

            return delegate(object val, Dictionary<string, object> data, ValidationHost host)
            {
                var common = _Common(val, opts);
                if (common != null)
                {
                    return common == false ? opts.Message : null;
                }

                if (db == null)
                {
                    db = host.Db;
                }

                if (table == null)
                {
                    table = host.Editor.Table()[0];
                }

                if (column == null)
                {
                    column = host.Field.DbField();
                }

                var query = db.Query("select")
                    .Table(table)
                    .Get(column)
                    .Where(column, val);

                // If doing an edit then we need to also discount the current row,
                // since it is of course already validly unique
                if (host.Action == "edit")
                {
                    query.Where(host.Editor.Pkey(), host.Id, "!=");
                }

                var res = query.Exec();

                return res.Count() == 0 ?
                    null :
                    opts.Message;
            };
        }


        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
         * Private methods
         */

        /// <summary>
        /// Perform common validation using the configuration parameters
        /// </summary>
        /// <param name="val">Value</param>
        /// <param name="opts">Validation options</param>
        /// <returns>null for invalid, true if immediately valid and false if invalid</returns>
        private static Boolean? _Common(object val, ValidationOpts opts)
        {
            // Error state tests
            if (!opts.Optional && val == null)
            {
                // Value must be given
                return false;
            }

            if (val != null && !opts.Empty && val.ToString() == "")
            {
                // Value must not be empty
                return false;
            }

            // Validate passed states
            if (opts.Optional && val == null)
            {
                return true;
            }

            if (opts.Empty && val.ToString() == "")
            {
                return true;
            }

            // Have the specific validation funciton perform its tests
            return null;
        }
    }
}
