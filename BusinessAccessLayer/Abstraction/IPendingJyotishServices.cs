using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IPendingJyotishServices
    {

        public  Task<bool> UploadDocumentAsync(DocumentViewModel model);
        public  DocumentModel Documents(string email);
        public  Task<JyotishModel> Profile(string email);
        public bool UpdateProfile(JyotishViewModel model , string path);
        public string Role(string Email);
    }
}
