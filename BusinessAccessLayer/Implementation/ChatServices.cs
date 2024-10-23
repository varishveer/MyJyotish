using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using ModelAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Implementation
{
    public  class ChatServices:IChat
    {
        private readonly ApplicationContext _context;

        public ChatServices(ApplicationContext context)
        {
            _context = context;
        }

        public string AddChat(ChatModel cm)
        {
            _context.Chat.Add(cm);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return "chat Added";
            }
            else
            {
                return "some error";
            }
        }

        public List<ChatModel> GetChats(int sender, int receiver)
        {
            var data = _context.Chat.Where(e => (e.senderId == sender && e.receiverId == receiver) || (e.senderId == receiver && e.receiverId == sender)).ToList();
            return data;
        }
    }
}
