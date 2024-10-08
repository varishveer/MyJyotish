using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IUserServices
    {
      
       
       
        public List<JyotishModel> GetAstroListCallChat(string ListName);
        public List<PoojaCategoryModel> GetAllPoojaCategory();
      /*  public List<PoojaRecordModel> GetPoojaList(int id);*/
        public PoojaRecordModel GetPoojaDetail(int PoojaId);
        public List<JyotishModel> TopAstrologer(string City);
        public List<JyotishModel> AllAstrologer();
        public JyotishModel AstrologerProfile(int Id);
        public List<JyotishModel> SearchAstrologer(string keyword);
        public List<IdImageViewModel> SliderImageList(string keyword);

    }
}
