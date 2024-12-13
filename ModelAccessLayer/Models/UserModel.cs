using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public  class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public DateOnly? DoB { get; set; }
        public string? PlaceOfBirth { get; set; }
        public int? Otp { get; set; }
        
        
        public string? Password { get; set; }
       
        public TimeOnly? TimeOfBirth { get; set; }
        public string? CurrentAddress { get; set; }
        public int? Country { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        public int? Pincode { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int? CountryCodeId { get; set; }
        public string? Status { get; set; }
        public CountryCode countryCoderecord { get; set; }
        public ICollection<CallingModel> CallingModelRecord { get; set; } = new List<CallingModel>();
        public ICollection<ChattingModel> ChattingModelRecord { get; set; } = new List<ChattingModel>();
        public ICollection<ChatedUser> ChatedUserRecord { get; set; } = new List<ChatedUser>();
        public ICollection<AppointmentModel> AppointmentRecord { get; set; } = new List<AppointmentModel>();
        public ICollection<UserWallet> UserRecord { get; set; } = new List<UserWallet>();
        public ICollection<UserPaymentRecordModel> UserPaymentRecords { get; set; } = new List<UserPaymentRecordModel>();
        public ICollection<JyotishUserAttachmentModel> JyotishUserAttachmentRecords { get; set; } = new List<JyotishUserAttachmentModel>(); 
        public ICollection<WalletHistoryModel> UserWalletHistoryRecords { get; set; } = new List<WalletHistoryModel>();
        public ICollection<ClientMembers> memebers { get; set; } = new List<ClientMembers>();
        public ICollection<JyotishRatingModel> JyotishRating { get; set; } = new List<JyotishRatingModel>();
        public ICollection<UserServiceRecordModel> UserServiceRecord { get; set; } = new List<UserServiceRecordModel>();
        public ICollection<BookedPoojaList> BookedPooja { get; set; } = new List<BookedPoojaList>();
        public ICollection<KundaliMatchingModel> KundaliMatching { get; set; } = new List<KundaliMatchingModel>();
    }
}
