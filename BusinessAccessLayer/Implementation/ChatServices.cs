﻿using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public string AddChatUser(ChatedUserViewModel cu)
        {
            var Records = _context.ChatedUser.Where(e => e.UserId == cu.UserId && e.JyotishId == cu.JyotishId).FirstOrDefault();


            if (Records!=null)
            {
                Records.LastMessageAt = cu.LastMessageAt;
                _context.ChatedUser.Update(Records);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return "Chated User Added";
                }
                else
                {
                    return "some error";
                }
            }
            else
            {
                ChatedUser data = new ChatedUser();
                data.FirstMessageAt = cu.FirstMessageAt;
                data.LastMessageAt = cu.LastMessageAt;
                data.Status = cu.Status;
                data.UserId = cu.UserId;
                data.JyotishId = cu.JyotishId;

                _context.ChatedUser.Add(data);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    return "Chated User Added";
                }
                else
                {
                    return "some error";
                }
            }
           
        }

        public List<ChatModel> GetChats(int sender, int receiver)
        {
            var data = _context.Chat.Where(e => (e.senderId == sender && e.receiverId == receiver) || (e.senderId == receiver && e.receiverId == sender)).OrderByDescending(e=>e.Id).Take(150).Reverse().ToList();
            return data;
        } 
        public dynamic GetChatedUser(int id, string userType)
        {
            dynamic data;
            if (userType.ToLower(  ) == "client")
            {
             data = (from chat in _context.ChatedUser
                    join Jyotish in _context.JyotishRecords on chat.JyotishId equals Jyotish.Id
                    where (chat.UserId == id && chat.Status==1) orderby (chat.LastMessageAt)
                    select new
                    {
                        Id = Jyotish.Id,
                        FirstMessageDate = chat.FirstMessageAt,
                        LastMessageDate = chat.LastMessageAt,
                        UserName = Jyotish.Name,  
                        Profile = Jyotish.ProfileImageUrl
                        
                    }).ToList();

            }
            else
            {
                data = (from chat in _context.ChatedUser
                        join Users in _context.Users on chat.UserId equals Users.Id
                        where (chat.JyotishId == id && chat.Status == 1)
                        orderby (chat.LastMessageAt)
                        select new
                        {
                            Id = Users.Id,
                            FirstMessageDate = chat.FirstMessageAt,
                            LastMessageDate = chat.LastMessageAt,
                            UserName = Users.Name,
                            Profile = Users.ProfilePictureUrl

                        }).ToList();

            }
            return data;
        }
    }
}
