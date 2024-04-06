﻿using DNA_CLI_Framework.Commands;

namespace DNA_CLI_Framework.CommandHandlers
{
    public class DefaultCommandHandler : CommandHandler
    {
        /// <summary>
        /// Initializes a new instance of the CommandHandler class
        /// </summary>
        /// <param name="commandPrefix"> The Prefix that Identifies a single Command </param>
        public DefaultCommandHandler() : base()
        {

        }

        /// <summary>
        /// Handles all the Commands passed in by the User
        /// </summary>
        /// <param name="args"> The Arguments passed by the User </param>
        public override void HandleCommands(string[] args)
        {
            if (args.Length == 0)
                return;

            string combinedArgs = string.Join(" ", args);
            string[] individualCommands = combinedArgs.Split(CommandPrefix);
            string defaultCommand = individualCommands[0].Trim();
            string[] commands = individualCommands.Skip(1).ToArray();
            bool isOnlyDefaultCommand = commands.Length == 0;

            HandleDefaultCommand(defaultCommand, isOnlyDefaultCommand);
            HandleAdditionalCommands(commands);
        }

        /// <summary>
        /// Handles the Default Command for the User
        /// </summary>
        /// <param name="commandArguments"> The Command Arguments for the Default Command </param>
        /// <param name="isOnlyDefaultCommand"> Flag Identifying if only the Default Command will be activated or if there are trailing Commands </param>
        protected override void HandleDefaultCommand(string commandArguments, bool isOnlyDefaultCommand)
        {
            string[] args = commandArguments.Split(" ");

            DefaultCommand? defaultCommand = CommandFactory.GetDefaultCommand();

            if (defaultCommand == null)
                return;

            if (isOnlyDefaultCommand)
                defaultCommand.ExecuteSolo(args);
            else
                defaultCommand.Execute(args);
        }

        /// <summary>
        /// Handles all Additonal Commands passed in by the User
        /// </summary>
        /// <param name="commands"> The Arguments for all the Additional Commands </param>
        protected override void HandleAdditionalCommands(string[] commands)
        {
            foreach (string command in commands)
            {
                string[] args = command.Split(" ");

                args = args.Where(arg => !string.IsNullOrWhiteSpace(arg)).ToArray();

                Command? commandInstance = CommandFactory.GetCommand(args[0]);

                if (commandInstance != null)
                    commandInstance.Execute(args.Skip(1).ToArray());
            }
        }
    }
}