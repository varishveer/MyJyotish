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
    public class UserServices:IUserServices
    {
        private readonly ApplicationContext _context;
        public UserServices(ApplicationContext context)
        {
            _context = context;
        }
        public List<string> HomaPageSlider()
        {
            var record = _context.Sliders.Select(x => x.HomePage).ToList();
            return record;
        }
        public List<JyotishModel> OurAstrologer()
        {
            List<JyotishModel> record = _context.JyotishRecords.ToList();
            return record;
        }
        

        public List<string> BAPCategorySlider()
        {
            var record = _context.Sliders.Select(x => x.BookPoojaCategory).ToList();
            return record;
        }
        public List<string> PoojaListSlider()
        {
            var record = _context.Sliders.Select(x => x.PoojaList).ToList();
            return record;
        }
        public List<JyotishModel> GetAstroListCallChat(string ListName)
        {
            if(ListName == "Chat")
            {
                var record = _context.JyotishRecords.Where(x=>x.Chat == true).ToList();    
                return record;
            }
            else if (ListName == "Call")
            {
                var record = _context.JyotishRecords.Where(x => x.Call == true).ToList();
                return record;
            }
            else { return null; }
        }

        public List<PoojaCategoryModel> GetAllPoojaCategory()
        {
            var record = _context.PoojaCategory.ToList();
            if(record == null)
            { return null; }
            else { return record; }
        }

        public List<PoojaRecordModel> GetPoojaList(int id)
        {
            var record = _context.PoojaRecord.Where(x=>x.PoojaCategoryId == id).ToList();
            if (record == null)
            { return null; }
            else { return record; }
        }
        
        public PoojaRecordModel GetPoojaDetail(int PoojaId)
        {
            var record = _context.PoojaRecord.Where(x => x.Id == PoojaId).FirstOrDefault(); 
            if (record == null)
            { return null; }    
            else { return record; }
        }

    }
}
