using System.Runtime.CompilerServices;
using Youtube_DL_Frontend.Data;


namespace Youtube_DL_Frontend.Parsing {
    internal class ParserInstance {
        public Enums.parsers parserName;
        private Dictionary<string, CommandParser.command> parser;
        private List<CommandParser.command> menuList;
        public ParserInstance(Enums.parsers parserName) {
            this.parserName = parserName;
            parser = new Dictionary<string, CommandParser.command>();
            menuList = new List<CommandParser.command>();
        }
        public ParserInstance(Enums.parsers parserName, List<CommandParser.command> commands) {
            this.parserName = parserName;
            parser = new Dictionary<string, CommandParser.command>();
            menuList = new List<CommandParser.command>();
            for (int i = 0; i < commands.Count(); i++) {
                string commandName = Statics.preProcessInput(commands[i].getCommandName());
                parser.Add(commandName, commands[i]);
                menuList.Add(commands[i]);
                parser.Add((i + 1).ToString(), commands[i]);
            }
        }
        public void registerCommand(string commandName, Action<PresetManager, RuntimeData>? action) {

            commandName = Statics.preProcessInput(commandName);
            parser.Add(commandName, new CommandParser.command(commandName, action));
            menuList.Add(new CommandParser.command(commandName, action));
            parser.Add(menuList.Count.ToString(), new CommandParser.command(commandName, action));
        }
        public void registerCommand(string commandName, Action<PresetManager, RuntimeData> action, Func<PresetManager, RuntimeData, string> dynamicDataLambda) {

            commandName = Statics.preProcessInput(commandName);
            parser.Add(commandName, new CommandParser.command(commandName, action, true, dynamicDataLambda));
            menuList.Add(new CommandParser.command(commandName, action, true, dynamicDataLambda));
            parser.Add(menuList.Count.ToString(), new CommandParser.command(commandName, action, true, dynamicDataLambda));
        }
        public bool registerAlias(string commandName, string alias) {

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
        public bool unregisterCommand(string commandName) {

            commandName = Statics.preProcessInput(commandName);
            CommandParser.command value;
            bool foundValue = parser.TryGetValue(commandName, out value);
            if (!foundValue) { return false; }

            List<string> aliases = value.getAliases();
            parser.Remove(commandName);
            menuList.Remove(value);

            for (int i = 0; i < aliases.Count(); i++) {
                parser.Remove(aliases[i]);
            }
            return true;
        }
        public bool processInput(string? input, PresetManager preset, RuntimeData runtime) {

            if (input == null) { return false; }

            input = Statics.preProcessInput(input);
            string[] inputArray = input.Split(" ");
            bool foundValue = parser.TryGetValue(inputArray[0], out CommandParser.command value);
            if (!foundValue) { return false; }
            value.invokeLambda(preset, runtime);
            if (value.hasDynamicData()) {
                generateMenu(preset, runtime);
            }
            return true;
        }
        public string generateMenu(PresetManager preset, RuntimeData runtime) {
            string[][] preList = new string[menuList.Count][];
            for (int i = 0; i < menuList.Count; i++) {
                preList[i] = new string[] {
                    (i + 1).ToString(),
                    menuList[i].getCommandName(),
                    menuList[i].getDynamicData(preset, runtime)};
            }
            return Interface.getAscii(1) + Statics.generateList("", preList);

        }
        public async Task<string> generateMenuAsync(PresetManager preset, RuntimeData runtime) {
            await Task.Run(() => {
                string[][] preList = new string[menuList.Count][];
                for (int i = 0; i < menuList.Count; i++) {
                    preList[i] = new string[] {
                    (i + 1).ToString(),
                    menuList[i].getCommandName(),
                    menuList[i].getDynamicData(preset, runtime)};
                }
                return Interface.getAscii(1) + Statics.generateList("", preList);
            });
            return "";
        }
    }
}