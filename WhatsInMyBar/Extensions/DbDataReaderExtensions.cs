﻿/*
Copyright (c) 2013, iD Commerce + Logistics
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted
provided that the following conditions are met:

Redistributions of source code must retain the above copyright notice, this list of conditions
and the following disclaimer. Redistributions in binary form must reproduce the above copyright
notice, this list of conditions and the following disclaimer in the documentation and/or other
materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsInMyBar.Extensions
{
    /// <summary>
    /// Provides extension methods for DbDataReader
    /// </summary>
    public static class DbDataReaderExtensions
    {
        /// <summary>
        /// Gets the value of the specified column, throwing an exception if the column is null.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="reader">The reader to get a value from.</param>
        /// <param name="name">The name of the reader.</param>
        /// <returns>The value of the column.</returns>
        public static T GetValue<T>(this DbDataReader reader, string name)
        {
            return GetValue<T>(reader, name, null);
        }

        /// <summary>
        /// Gets the value of the specified column, throwing an exception if the column is null.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="reader">The reader to get a value from.</param>
        /// <param name="name">The name of the reader.</param>
        /// <param name="nullAction">An action to call when the <paramref name="reader"/> returns a null value.</param>
        /// <returns>The value of the column.</returns>
        public static T GetValue<T>(this DbDataReader reader, string name, Action<string> nullAction)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (name == null) throw new ArgumentNullException("name");

            int ordinal = reader.GetOrdinal(name);

            if (reader.IsDBNull(ordinal))
            {
                if (nullAction != null)
                {
                    nullAction(name);
                }

                throw new InvalidCastException(string.Format("Unable to read value for '{0}', database returned null.", name));
            }

            return (T)reader.GetValue(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column, or default(<typeparamref name="T"/>) if the column is null.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="reader">The reader to get a value from.</param>
        /// <param name="name">The name of the reader.</param>
        /// <returns>If the column is null, default(<typeparamref name="T"/>). Otherwise, the value of the column.</returns>
        public static T GetValueOrDefault<T>(this DbDataReader reader, string name)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (name == null) throw new ArgumentNullException("name");

            int ordinal = reader.GetOrdinal(name);
            return reader.IsDBNull(ordinal) ? default(T) : (T)reader.GetValue(ordinal);
        }

        /// <summary>
        /// Gets the value of the specified column, or a default value if the column is null.
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="reader">The reader to get a value from.</param>
        /// <param name="name">The name of the reader.</param>
        /// <param name="defaultValue">The default value to return if the column is null.</param>
        /// <returns>If the column is null, <paramref name="defaultValue"/>. Otherwise, the value of the column.</returns>
        public static T GetValueOrDefault<T>(this DbDataReader reader, string name, T defaultValue)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (name == null) throw new ArgumentNullException("name");

            int ordinal = reader.GetOrdinal(name);
            return reader.IsDBNull(ordinal) ? defaultValue : (T)reader.GetValue(ordinal);
        }
    }
}