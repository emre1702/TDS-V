using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TDS_Client.Data.Defaults;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class BrowserHandlerBase : ServiceBase
    {
        private readonly LinkedList<Action> _executeList = new LinkedList<Action>();
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly string _url;

        protected BrowserHandlerBase(LoggingHandler loggingHandler, string url)
            : base(loggingHandler)
        {
            _url = url;
        }

        public HtmlWindow Browser { get; private set; }

        public void CreateBrowser()
        {
            Browser = new HtmlWindow(_url);
            RAGE.Ui.Console.Log(ConsoleVerbosity.Info, $"Browser for URL {_url} loaded: {(!(Browser is null))} | Active: {Browser.Active} | Id: {Browser.Id}");
            ProcessExecuteList();
        }

        public virtual void SetReady(params object[] args)
        {
            Execute(ToBrowserEvent.InitLoadAngular, args);
            Browser.Active = true;
        }

        public void Stop()
        {
            if (Browser is null)
                return;
            Browser.Destroy();
            Browser = null;
            _executeList.Clear();
        }

        protected void Execute(string eventName, params object[] args)
        {
            string execStr = GetExecStr(eventName, args);
            ExecuteStr(execStr);
        }

        protected void ExecuteFast(string eventName, params object[] args)
        {
            if (Browser is null)
                _executeList.AddLast(() => Browser.Call(eventName, args));
            else
                Browser.Call(eventName, args);
        }

        protected void ExecuteStr(string str)
        {
            if (Browser is null)
                _executeList.AddLast(() => Browser.ExecuteJs(str));
            else
                Browser.ExecuteJs(str);
        }

        protected string GetExecStr(string eventName, params object[] args)
        {
            if (args.Length == 0)
                return $"RageAngularEvent(`{eventName}`)";

            _stringBuilder.Clear();
            var strBuilder = _stringBuilder.Append($"RageAngularEvent(`{eventName}`");
            foreach (var arg in args)
            {
                var argType = arg?.GetType();
                if (arg is null)
                    strBuilder.Append(", undefined");
                else if (arg is string)
                    if (arg != null)
                        strBuilder.Append($", `{arg}`");
                    else
                        strBuilder.Append(", undefined");
                else if (argType.IsEnum)
                    strBuilder.Append($", {Convert.ChangeType(arg, Type.GetTypeCode(argType))}");
                else if (!argType.IsValueType)
                    strBuilder.Append($", `{Serializer.ToBrowser(arg)}`");
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
                    strBuilder.Append($", {arg}");
            }

            strBuilder.Append(")");

            return strBuilder.ToString();
        }

        protected void ProcessExecuteList()
        {
            foreach (var exec in _executeList)
            {
                exec();
            }
            _executeList.Clear();
        }
    }
}
