using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WhatsInMyBar.Extensions
{
    public static class DbCommandExtensionMethods
    {
        public static void AddParameter(this IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}