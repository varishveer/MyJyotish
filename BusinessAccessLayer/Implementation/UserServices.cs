using BusinessAccessLayer.Abstraction;
using DataAccessLayer.DbServices;
using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationContext _context;
        public UserServices(ApplicationContext context)
        {
            _context = context;
        }





        public List<JyotishModel> GetAstroListCallChat(string ListName)
        {
            if (ListName == "Chat")
            {
                var record = _context.JyotishRecords.Where(x => x.Chat == true).ToList();
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
            if (record == null)
            { return null; }
            else { return record; }
        }

       /* public List<PoojaRecordModel> GetPoojaList(int id)
        {
            var record = _context.PoojaRecord.Where(x => x.PoojaCategoryId == id).ToList();
            if (record == null)
            { return null; }
            else { return record; }
        }*/

        public PoojaRecordModel GetPoojaDetail(int PoojaId)
        {
            var record = _context.PoojaRecord.Where(x => x.Id == PoojaId).FirstOrDefault();
            if (record == null)
            { return null; }
            else { return record; }
        }


        public List<JyotishModel> TopAstrologer(string City)
        {
            var records = _context.JyotishRecords.Where(a => a.Status == "Complete").Where(x => x.City.Contains(City)).ToList();
            if (records.Count == 0)
            {
                records = _context.JyotishRecords.Where(x => x.Role == "Jyotish").Where(x => x.Country.Contains("India")).ToList();
            }
            return records;
        }
        public List<JyotishModel> AllAstrologer()
        {
            List<JyotishModel> record = _context.JyotishRecords.Where(x => x.Status == "Complete").ToList();
            return record;
        }

        public JyotishModel AstrologerProfile(int Id)
        {
            var record = _context.JyotishRecords.Where(x => x.Status == "Complete").Where(x => x.Id == Id).FirstOrDefault();
            return record;
        }
        public List<JyotishModel> SearchAstrologer(string keyword)
        {
            var result = _context.JyotishRecords.Where(x => x.Status == "Complete")
           .Where(record => record.Name.Contains(keyword) ||
                            record.Expertise.Contains(keyword) ||
                            record.Language.Contains(keyword) ||
                            record.Country.Contains(keyword) ||
                            record.State.Contains(keyword) ||
                            record.City.Contains(keyword))
           .ToList();

            return result;
        }


        public List<IdImageViewModel> SliderImageList(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }

            IQueryable<IdImageViewModel> query = null;

            switch (keyword)
            {
                case "HomePage":
                    query = _context.Sliders.Select(s => new IdImageViewModel
                    {
                        Id = s.Id,
                        ImageUrl = s.HomePage 
                    });
                    break;
                case "BookPoojaCategory":
                    query = _context.Sliders.Select(s => new IdImageViewModel
                    {
                        Id = s.Id,
                        ImageUrl = s.BookPoojaCategory 
                    });
                    break;
                case "PoojaList":
                    query = _context.Sliders.Select(s => new IdImageViewModel
                    {
                        Id = s.Id,
                        ImageUrl = s.PoojaList 
                    });
                    break;
                default:
                    return null; 
            }

            var records = query.ToList();
            return records.Count > 0 ? records : null;
        }

    }
}
