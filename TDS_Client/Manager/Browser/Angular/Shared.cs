﻿using Newtonsoft.Json;
using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TDS_Client.Manager.Browser.Angular
{
    static class Shared
    {

        public static string GetExecStr(string eventName, params object[] args)
        {
            var strBuilder = new StringBuilder($"RageAngularEvent(`{eventName}`");
            foreach (var arg in args)
            {
                if (arg is null)
                    strBuilder.Append(", undefined");
                else if (arg is string)
                    if (arg != null)
                        strBuilder.Append($", `{arg}`");
                    else
                        strBuilder.Append(", undefined");
                else if (!arg.GetType().IsValueType)
                    strBuilder.Append($", `{JsonConvert.SerializeObject(arg)}`");
                else if (arg is char)
                    strBuilder.Append($", '{arg}'");
                else if (arg is bool b)
                    strBuilder.Append($", {(b ? 1 : 0)}");
                else if (arg is float f)
                {
                    f = (float)Math.Floor(f * 100) / 100;
                    strBuilder.Append($", {f.ToString(CultureInfo.InvariantCulture)}");
                }
                else if (arg is double d)
                {
                    d = Math.Floor(d * 100) / 100;
                    strBuilder.Append($", {d.ToString(CultureInfo.InvariantCulture)}");
                }
                else if (arg is decimal de)
                {
                    de = Math.Floor(de * 100) / 100;
                    strBuilder.Append($", {de.ToString(CultureInfo.InvariantCulture)}");
                }
                else
                    strBuilder.Append($", {arg.ToString()}");
            }

            strBuilder.Append(")");

            return strBuilder.ToString();
        }
    }
}
