using ModelAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IUserServices
    {
        public List<string> HomaPageSlider();
        public List<JyotishModel> OurAstrologer();
        public List<string> BAPCategorySlider();
        public List<string> PoojaListSlider();
        public List<JyotishModel> GetAstroListCallChat(string ListName);
        public List<PoojaCategoryModel> GetAllPoojaCategory();
        public List<PoojaRecordModel> GetPoojaList(int id);
        public PoojaRecordModel GetPoojaDetail(int PoojaId);
    }
}
