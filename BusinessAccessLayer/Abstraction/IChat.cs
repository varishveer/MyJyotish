using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IChat
    {
        public string AddChat(ChatModel chat);
      public string AddChatUser(ChatedUserViewModel cu);
      public dynamic GetChatedUser(int id,string userType);
        public List<ChatModel> GetChats(int sender, int receiver);
    }
}
