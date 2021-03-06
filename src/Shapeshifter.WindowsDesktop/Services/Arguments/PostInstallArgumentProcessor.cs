﻿namespace Shapeshifter.WindowsDesktop.Services.Arguments
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Interfaces;
	using Serilog;

	using Shapeshifter.WindowsDesktop.Services.Files.Interfaces;
	using Shapeshifter.WindowsDesktop.Services.Processes.Interfaces;

	class PostInstallArgumentProcessor : ISingleArgumentProcessor
	{
		readonly ILogger logger;
		readonly IFileManager fileManager;
		readonly IProcessManager processManager;

		public bool Terminates => true;

		public PostInstallArgumentProcessor(
			ILogger logger,
			IFileManager fileManager,
			IProcessManager processManager)
		{
			this.logger = logger;
			this.fileManager = fileManager;
			this.processManager = processManager;
		}

		public bool CanProcess(string[] arguments)
		{
			return arguments.Contains("postinstall");
		}

		public async Task ProcessAsync(string[] arguments)
		{
			var updateIndex = Array.IndexOf(arguments, "postinstall");
			var targetDirectory = arguments[updateIndex + 1];

			logger.Information("Attempting to delete file {file}.", targetDirectory);
			await fileManager.DeleteFileIfExistsAsync(targetDirectory);
			logger.Verbose("Old file has been cleaned up.");

			processManager.LaunchFileWithAdministrativeRights(processManager.GetCurrentProcessFilePath());
		}
	}
}
