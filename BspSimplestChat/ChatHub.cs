using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BspSimplestChat
{
  [HubName("ChatHub")]
  public class ChatHub : Hub
  {
    [HubMethodName("Send")]
    public void Send(string message)
    {
      this.Clients.All.MessageReceived(message);
    }

  }
}