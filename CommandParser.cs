using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;

namespace Youtube_DL_Frontend
{
    internal class CommandParser
    {
        private Dictionary<string, CommandParser.command> parserInternal;
        private Dictionary<string, CommandParser.command> parserExternal;
        private Dictionary<string, CommandParser.command> parserMenu;
        private List<CommandParser.command> menuList;

        public enum commandScope
        {
            intern,
            external,
            menu
        }
        struct command
        {

            string commandName;
            List<string> aliases;
            Action<DatabaseObject, RuntimeData> lambda;
            bool dynamicData;
            Func<DatabaseObject, RuntimeData, string>? dynamicDataLambda;
            public command(string commandName, Action<DatabaseObject, RuntimeData> lambda, bool dynamicData = false, Func<DatabaseObject, RuntimeData, string>? dynamicDataLambda = null)
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
                lambda.Invoke(data, runtime);
            }
        }

        public CommandParser()
        {
            parserInternal = new Dictionary<string, CommandParser.command>();
            parserExternal = new Dictionary<string, CommandParser.command>();
            parserMenu = new Dictionary<string, CommandParser.command>();
            menuList = new List<CommandParser.command>();
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

        public void registerInternalCommand(string commandName, Action<DatabaseObject, RuntimeData> action)
        {

            commandName = Statics.preProcessInput(commandName);
            parserInternal.Add(commandName, new command(commandName, action));
        }
        public void registerExternalCommand(string commandName, Action<DatabaseObject, RuntimeData> action)
        {

            commandName = Statics.preProcessInput(commandName);
            parserInternal.Add(commandName, new command(commandName, action));
        }
        public void registerMenuCommand(string commandName, Action<DatabaseObject, RuntimeData> action)
        {

            commandName = Statics.preProcessInput(commandName);
            parserMenu.Add(commandName, new command(commandName, action));
            menuList.Add(new command(commandName, action));
            registerAlias(commandName, menuList.Count.ToString(), CommandParser.commandScope.menu);
        }
        public void registerInternalCommand(string commandName, Action<DatabaseObject, RuntimeData> action, Func<DatabaseObject, RuntimeData, string> dynamicDataLambda)
        {

            commandName = Statics.preProcessInput(commandName);
            parserInternal.Add(commandName, new command(commandName, action, true, dynamicDataLambda));
        }
        public void registerExternalCommand(string commandName, Action<DatabaseObject, RuntimeData> action, Func<DatabaseObject, RuntimeData, string> dynamicDataLambda)
        {

            commandName = Statics.preProcessInput(commandName);
            parserInternal.Add(commandName, new command(commandName, action, true, dynamicDataLambda));
        }
        public void registerMenuCommand(string commandName, Action<DatabaseObject, RuntimeData> action, Func<DatabaseObject, RuntimeData, string> dynamicDataLambda)
        {

            commandName = Statics.preProcessInput(commandName);
            parserMenu.Add(commandName, new command(commandName, action, true, dynamicDataLambda));
            menuList.Add(new command(commandName, action, true, dynamicDataLambda));
            registerAlias(commandName, menuList.Count.ToString(), CommandParser.commandScope.menu);
        }

        public bool registerAlias(string commandName, string alias, commandScope scope)
        {

            commandName = Statics.preProcessInput(commandName);
            bool foundValue = false;
            command value;
            switch (scope)
            {
                case commandScope.intern:
                    foundValue = parserInternal.TryGetValue(commandName, out value);
                    break;
                case commandScope.external:
                    foundValue = parserExternal.TryGetValue(commandName, out value);
                    break;
                case commandScope.menu:
                    foundValue = parserMenu.TryGetValue(commandName, out value);
                    break;
                default:
                    return false;
            }
            if (!foundValue) { return false; }

            alias = Statics.preProcessInput(alias);
            value.addAlias(alias);
            switch (scope)
            {
                case commandScope.intern:
                    parserInternal.Add(alias, value);
                    break;
                case commandScope.external:
                    parserExternal.Add(alias, value);
                    break;
                case commandScope.menu:
                    parserMenu.Add(alias, value);
                    break;
            }
            return true;
        }

        public bool unregisterInternalCommand(string commandName)
        {

            commandName = Statics.preProcessInput(commandName);
            command value;
            bool foundValue = parserInternal.TryGetValue(commandName, out value);
            if (!foundValue) { return false; }

            List<string> aliases = value.getAliases();
            parserInternal.Remove(commandName);

            for (int i = 0; i < aliases.Count(); i++)
            {
                parserInternal.Remove(aliases[i]);
            }
            return true;
        }
        public bool unregisterExternalCommand(string commandName)
        {

            commandName = Statics.preProcessInput(commandName);
            command value;
            bool foundValue = parserExternal.TryGetValue(commandName, out value);
            if (!foundValue) { return false; }

            List<string> aliases = value.getAliases();
            parserExternal.Remove(commandName);

            for (int i = 0; i < aliases.Count(); i++)
            {
                parserExternal.Remove(aliases[i]);
            }
            return true;
        }
        public bool unregisterMenuCommand(string commandName)
        {

            commandName = Statics.preProcessInput(commandName);
            command value;
            bool foundValue = parserMenu.TryGetValue(commandName, out value);
            if (!foundValue) { return false; }

            List<string> aliases = value.getAliases();
            parserMenu.Remove(commandName);

            for (int i = 0; i < aliases.Count(); i++)
            {
                parserMenu.Remove(aliases[i]);
            }
            return true;
        }

        public bool processInternalInput(string? input, DatabaseObject data, RuntimeData runtime)
        {

            if (input == null) { return false; }

            input = Statics.preProcessInput(input);
            string[] inputArray = input.Split(" ");
            bool foundValue = parserInternal.TryGetValue(inputArray[0], out command value);
            if (!foundValue) { return false; }
            value.invokeLambda(data, runtime);
            return true;
        }
        public bool processExternalInput(string? input, DatabaseObject data, RuntimeData runtime)
        {

            if (input == null) { return false; }

            input = Statics.preProcessInput(input);
            string[] inputArray = input.Split(" ");
            bool foundValue = parserExternal.TryGetValue(inputArray[0], out command value);
            if (!foundValue) { return false; }
            value.invokeLambda(data, runtime);
            return true;
        }
        public bool processMenuInput(string? input, DatabaseObject data, RuntimeData runtime)
        {

            if (input == null) { return false; }

            input = Statics.preProcessInput(input);
            string[] inputArray = input.Split(" ");
            bool foundValue = parserMenu.TryGetValue(inputArray[0], out command value);
            if (!foundValue) { return false; }
            value.invokeLambda(data, runtime);
            if (value.hasDynamicData())
            {
                generateMenu(data, runtime);
            }
            return true;
        }

        public void generateMenu(DatabaseObject data, RuntimeData runtime)
        {
            string[][] preList = new string[menuList.Count][];
            for (int i = 0; i < menuList.Count; i++)
            {
                preList[i] = new string[] {
                    (i + 1).ToString(),
                    menuList[i].getCommandName(),
                    menuList[i].getDynamicData(data, runtime)};
            }
            runtime.currentMenu = Interface.getAscii(1) + Statics.generateList("Youtube-DL FE Menu", preList);

        }
        public async void generateMenuAsync(DatabaseObject data, RuntimeData runtime)
        {
            await Task.Run(() =>
            {
                string[][] preList = new string[menuList.Count][];
                for (int i = 0; i < menuList.Count; i++)
                {
                    preList[i] = new string[] {
                    (i + 1).ToString(),
                    menuList[i].getCommandName(),
                    menuList[i].getDynamicData(data, runtime)};
                }
                runtime.currentMenu = Interface.getAscii(1) + Statics.generateList("Youtube-DL FE Menu", preList);
            });
        }
    }
}