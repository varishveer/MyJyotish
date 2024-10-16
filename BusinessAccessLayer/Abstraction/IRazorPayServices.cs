using ModelAccessLayer.Models;
using ModelAccessLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccessLayer.Abstraction
{
    public interface IRazorPayServices
    {

        public bool Order(PaymentCreateOrderViewModel model);
        public bool LogPayment(PaymentCaptureModel model, string status);
        public bool LogFailedPayment(PaymentCaptureModel model);
    }
}
