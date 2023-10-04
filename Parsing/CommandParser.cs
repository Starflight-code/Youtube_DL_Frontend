using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using Youtube_DL_Frontend.Data;

namespace Youtube_DL_Frontend.Parsing
{
    internal class CommandParser
    {
        private Dictionary<string, CommandParser.command> parserInternal;
        private Dictionary<string, CommandParser.command> parserExternal;
        private Dictionary<string, CommandParser.command> parserMenu;
        private List<CommandParser.command> menuList;
        public ParserInstance menu;
        public ParserInstance settings;
        public ParserInstance presets;

        public enum commandScope
        {
            intern,
            external,
            menu
        }
        public struct command
        {

            string commandName;
            List<string> aliases;
            Action<DatabaseObject, RuntimeData>? lambda;
            bool dynamicData;
            Func<DatabaseObject, RuntimeData, string>? dynamicDataLambda;
            public command(string commandName, Action<DatabaseObject, RuntimeData>? lambda, bool dynamicData = false, Func<DatabaseObject, RuntimeData, string>? dynamicDataLambda = null)
            {
                this.commandName = commandName;
                aliases = new List<string>();
                this.lambda = lambda;
                this.dynamicData = dynamicData;
                this.dynamicDataLambda = dynamicDataLambda;
            }
            public string getCommandName()
            {
                return commandName;
            }
            public bool hasDynamicData()
            {
                return dynamicData;
            }

            public string getDynamicData(DatabaseObject data, RuntimeData runtime)
            {
                if (dynamicDataLambda == null)
                {
                    return "";
                }
                string dynamicData = dynamicDataLambda.Invoke(data, runtime);
                if (dynamicData == null)
                {
                    dynamicData = "";
                }
                return dynamicData;
            }
            public void addAlias(string alias)
            {
                aliases.Add(alias);
            }
            public List<string> getAliases()
            {
                return aliases;
            }

            public void invokeLambda(DatabaseObject data, RuntimeData runtime)
            {
                if (lambda == null) { return; }
                lambda.Invoke(data, runtime);
            }
        }

        public CommandParser()
        {
            parserInternal = new Dictionary<string, CommandParser.command>();
            parserExternal = new Dictionary<string, CommandParser.command>();
            parserMenu = new Dictionary<string, CommandParser.command>();
            menuList = new List<CommandParser.command>();
            menu = new ParserInstance(Enums.parsers.main);
            settings = new ParserInstance(Enums.parsers.settings);
            presets = new ParserInstance(Enums.parsers.presets);
        }

        public string[] getArgs(string[] fullCommand)
        {
            string[] returnArray = new string[fullCommand.Length - 1];
            for (int i = 1; i < fullCommand.Length; i++)
            {
                returnArray[i - 1] = fullCommand[i];
            }
            return returnArray;
        }
        public void registerMenuCommand(string commandName, Action<DatabaseObject, RuntimeData> action)
        {
            menu.registerCommand(commandName, action);
        }
        public void registerMenuCommand(string commandName, Action<DatabaseObject, RuntimeData> action, Func<DatabaseObject, RuntimeData, string> dynamicDataLambda)
        {
            menu.registerCommand(commandName, action, dynamicDataLambda);
        }
        public bool processMenuInput(string? input, DatabaseObject data, RuntimeData runtime)
        {
            return menu.processInput(input, data, runtime);
        }

        public void generateMenu(DatabaseObject data, RuntimeData runtime)
        {
            runtime.currentMenu = menu.generateMenu(data, runtime);
        }
        public async void generateMenuAsync(DatabaseObject data, RuntimeData runtime)
        {
            runtime.currentMenu = await menu.generateMenuAsync(data, runtime);
        }
    }
}