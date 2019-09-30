using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 表示从命令行中接收到的参数
    /// </summary>
    public abstract class CommandLineArgs
    {
        private List<string> commandLineArgs = new List<string>();
        private Dictionary<string, CommandParser> commandParsers = new Dictionary<string, CommandParser>(StringComparer.OrdinalIgnoreCase);

        protected CommandLineArgs()
        {
            RegisterCommandLineParsers();
            ParseCommandLineArgs();
        }

        protected abstract void RegisterCommandLineParsers();

        /// <summary>
        /// 从命令行参数中读取一个参数值
        /// </summary>
        /// <returns></returns>
        protected string ReadValue()
        {
            if (commandLineArgs.Count == 0) return string.Empty;

            var s = commandLineArgs[0];
            commandLineArgs.RemoveAt(0);

            return s;
        }

        /// <summary>
        /// 从命令行参数中读取一个数组
        /// </summary>
        /// <returns></returns>
        protected string[] ReadValues()
        {
            var s = ReadValue();
            if (s == null) s = string.Empty;

            return s.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 注册命令行指令解析器
        /// </summary>
        /// <param name="commandName">指令名称，不区分大小写</param>
        /// <param name="parser">指令解析函数</param>
        protected void RegisterCommandLineParser(string commandName, Action parser)
        {
            RegisterCommandLineParser(commandName, 1, parser);
        }

        /// <summary>
        /// 注册命令行指令解析器
        /// </summary>
        /// <param name="commandName">指令名称，不区分大小写</param>
        /// <param name="argsCount">指令参数数量</param>
        /// <param name="parser">指令解析函数</param>
        protected void RegisterCommandLineParser(string commandName, int argsCount, Action parser)
        {
            commandParsers.Add(commandName, new CommandParser(commandName, argsCount, parser));
        }

        private void ParseCommandLineArgs()
        {
            commandLineArgs = Environment.GetCommandLineArgs().ToList();
            if (commandLineArgs.Count > 0) commandLineArgs.RemoveAt(0);  //第一个参数为应用程序的路径，将其移除

            if (commandLineArgs.Count == 0) return;

            //进行命令行的解析
            while (commandLineArgs.Count > 0)
            {
                var commandName = ReadValue();

                if (commandName.StartsWith("/") || commandName.StartsWith("-"))  //命名名称以/或-开头
                {
                    commandName = commandName.Substring(1);  //去掉命名前的前缀，得到命令名称
                    if (commandParsers.ContainsKey(commandName))
                    {
                        var parser = commandParsers[commandName];
                        if (commandParsers.Count >= parser.ArgsCount)
                        {
                            parser.Parse();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 命令行指令解析器信息
        /// </summary>
        private class CommandParser
        {
            internal CommandParser(string commandName, int argsCount, Action parse)
            {
                this.CommandName = commandName;
                this.ArgsCount = argsCount;
                this.Parse = parse;
            }

            /// <summary>
            /// 指令名称
            /// </summary>
            public string CommandName { get; private set; }

            /// <summary>
            /// 指令要求的参数数量
            /// </summary>
            public int ArgsCount { get; private set; }

            /// <summary>
            /// 指令参数解析函数
            /// </summary>
            public Action Parse { get; private set; }
        }
    }
}
