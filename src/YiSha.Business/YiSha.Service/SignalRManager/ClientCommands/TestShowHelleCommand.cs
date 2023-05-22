using Koo.SocketCommands;
using Koo.Utilities.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Service.SignalRManager.ClientCommands
{
    public class TestShowHelleCommand : BaseSocketCommand, IClientCommand
    {
        protected override void ExecuteOverride(Message msg)
        {
            var xxx = msg;
        }
    }
}
