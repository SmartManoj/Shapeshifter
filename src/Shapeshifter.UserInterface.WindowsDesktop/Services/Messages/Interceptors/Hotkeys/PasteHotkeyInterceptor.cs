﻿using Shapeshifter.UserInterface.WindowsDesktop.Infrastructure.Environment.Interfaces;
using Shapeshifter.UserInterface.WindowsDesktop.Infrastructure.Logging.Interfaces;
using Shapeshifter.UserInterface.WindowsDesktop.Services.Api;
using Shapeshifter.UserInterface.WindowsDesktop.Services.Events;
using Shapeshifter.UserInterface.WindowsDesktop.Services.Keyboard.Interfaces;
using Shapeshifter.UserInterface.WindowsDesktop.Services.Messages.Factories.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using Shapeshifter.UserInterface.WindowsDesktop.Services.Messages.Interfaces;

namespace Shapeshifter.UserInterface.WindowsDesktop.Services.Keyboard
{
    [ExcludeFromCodeCoverage]
    class PasteHotkeyInterceptor : IPasteHotkeyInterceptor
    {
        readonly ILogger logger;
        readonly IHotkeyInterception hotkeyInterception;

        public event EventHandler<PasteHotkeyFiredArgument> HotkeyFired;

        public PasteHotkeyInterceptor(
            ILogger logger,
            IHotkeyInterceptionFactory hotkeyInterceptionFactory)
        {
            this.logger = logger;

            hotkeyInterception = hotkeyInterceptionFactory.CreateInterception(
                KeyboardApi.VK_KEY_V, true, true);
        }

        public void Install(IntPtr windowHandle)
        {
            hotkeyInterception.Start(windowHandle);
        }

        public void Uninstall(IntPtr windowHandle)
        {
            hotkeyInterception.Stop(windowHandle);
        }

        public void ReceiveMessageEvent(WindowMessageReceivedArgument e)
        {
            if (e.Message == KeyboardApi.WM_HOTKEY && (int)e.WordParameter == hotkeyInterception.InterceptionId)
            {
                logger.Information("Hotkey message received.");

                if (HotkeyFired != null)
                {
                    HotkeyFired(this, new PasteHotkeyFiredArgument());
                }
            }
        }
    }
}
