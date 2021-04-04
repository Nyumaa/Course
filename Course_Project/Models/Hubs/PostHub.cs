using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Project.Models.Hubs
{
    public class PostHub : Hub
    {
        public async Task Comment(string author, string postId, string comment)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveComment", author, postId, comment);
        }
    }
}
