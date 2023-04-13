using Microsoft.AspNetCore.SignalR;
using MyChatRoom.Services;
using System;
using System.Threading.Tasks;

namespace MyChatRoom.Hubs
{
    public class ChatHub : Hub
    {
        public readonly ChatService _chatService;
        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "MyChatRoom");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "MyChatRomm");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string name)
        {
            _chatService.AddUserConnectionId(name, Context.ConnectionId);
            var onlineUsers = _chatService.GetOnlineUsers();
            await Clients.Groups("MyChatRoom").SendAsync("OnlineUsers", onlineUsers);
        }
    }
}
