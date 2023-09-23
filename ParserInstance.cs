namespace Youtube_DL_Frontend
{
    internal class ParserInstance
    {
        private Dictionary<string, CommandParser.command> parser;
        private List<CommandParser.command> menuList;
        ParserInstance()
        {
            parser = new Dictionary<string, CommandParser.command>();
            menuList = new List<CommandParser.command>();
        }
        public void registerCommand(string commandName, Action<DatabaseObject, RuntimeData> action)
        {

            commandName = Statics.preProcessInput(commandName);
            parser.Add(commandName, new CommandParser.command(commandName, action));
            menuList.Add(new CommandParser.command(commandName, action));
        }
        public void registerCommand(string commandName, Action<DatabaseObject, RuntimeData> action, Func<DatabaseObject, RuntimeData, string> dynamicDataLambda)
        {

            commandName = Statics.preProcessInput(commandName);
            parser.Add(commandName, new CommandParser.command(commandName, action, true, dynamicDataLambda));
            menuList.Add(new CommandParser.command(commandName, action, true, dynamicDataLambda));
        }
        public bool registerAlias(string commandName, string alias)
        {

            commandName = Statics.preProcessInput(commandName);
            bool foundValue = false;
            CommandParser.command value;

            foundValue = parser.TryGetValue(commandName, out value);
            if (!foundValue) { return false; }

            alias = Statics.preProcessInput(alias);
            value.addAlias(alias);

            parser.Add(alias, value);
            return true;
        }
        public bool unregisterCommand(string commandName)
        {

            commandName = Statics.preProcessInput(commandName);
            CommandParser.command value;
            bool foundValue = parser.TryGetValue(commandName, out value);
            if (!foundValue) { return false; }

            List<string> aliases = value.getAliases();
            parser.Remove(commandName);
            menuList.Remove(value);

            for (int i = 0; i < aliases.Count(); i++)
            {
                parser.Remove(aliases[i]);
            }
            return true;
        }
        public bool processInput(string? input, DatabaseObject data, RuntimeData runtime)
        {

            if (input == null) { return false; }

            input = Statics.preProcessInput(input);
            string[] inputArray = input.Split(" ");
            bool foundValue = parser.TryGetValue(inputArray[0], out CommandParser.command value);
            if (!foundValue) { return false; }
            value.invokeLambda(data, runtime);
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
            runtime.currentMenu = Interface.getAscii(1) + Statics.generateList("", preList);

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
                runtime.currentMenu = Interface.getAscii(1) + Statics.generateList("", preList);
            });
        }
    }
}