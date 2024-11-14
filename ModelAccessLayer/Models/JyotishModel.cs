using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelAccessLayer.Models
{
    public class JyotishModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Mobile { get; set; }
        public string? AlternateMobile { get; set; }

        public string Role { get; set; }

        public string? Name { get; set; }
       
        public string? Gender { get; set; }

        public string? Language { get; set; }
      
        public string? Expertise { get; set; }
        
       
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        [JsonIgnore]
        public string? Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? ProfileImageUrl { get; set; }
       
        public string? ApprovedStatus { get; set; }
        public bool Status { get; set; }
        public bool NewStatus { get; set; }


        public int? Otp { get; set; }


        public int? Experience { get; set; }
        public bool? Pooja { get; set; }
        
        public bool? Call { get; set; } 
        public int? CallCharges { get; set; }
        public bool? Chat { get; set; }
        public int? ChatCharges { get; set; }
        public int? AppointmentCharges { get; set; }
        public string? Address { get; set; }
        public TimeOnly? TimeTo { get; set; }
        public TimeOnly? TimeFrom { get; set; }
        public string? About { get; set; }
        public string? AwordsAndAchievement { get; set; }
        public string? Specialization { get; set; }
        public string? Rating { get; set; }
        public string? SuccessRate { get; set; }
        public int? Pincode { get; set; }

        public string? Message { get; set; }
        // public int? TempRecordId { get; set; }

        [JsonIgnore]
        public DocumentModel DocumentModel { get; set; }
        [JsonIgnore]
        public ICollection<CallingModel> CallingModelRecord { get; set; } = new List<CallingModel>();
        [JsonIgnore]
        public ICollection<ChattingModel> ChattingModelRecord { get; set; } = new List<ChattingModel>();
        [JsonIgnore]
        public ICollection<SlotBookingModel> Slots { get; set; }
        [JsonIgnore]
        public ICollection<JyotishGalleryModel> JyotishGallery { get; set; }
        [JsonIgnore]
        public ICollection<JyotishVideosModel> JyotishVideos { get; set; }
        [JsonIgnore]

        public ICollection<ChatedUser> ChatedUserRecord { get; set; } = new List<ChatedUser>();
        [JsonIgnore]
        public ICollection<AppointmentModel> AppointmentRecord { get; set; } = new List<AppointmentModel>();
        [JsonIgnore]
        public ICollection<jyotishWallet> JyotishRecord { get; set; } = new List<jyotishWallet>();
        [JsonIgnore]
        public ICollection<JyotishPaymentRecordModel> jyotishPaymentRecords { get; set; } = new List<JyotishPaymentRecordModel>();
        [JsonIgnore]

        public ICollection<JyotishUserAttachmentModel> JyotishUserAttachmentRecords { get; set; } = new List<JyotishUserAttachmentModel>();
        [JsonIgnore]
        public ICollection<WalletHistoryModel> JytoishWalletHistoryRecord { get; set; } = new List<WalletHistoryModel>();
        public ICollection<AppointmentSlotModel> AppointmentSlotData { get; set; } = new List<AppointmentSlotModel>();

    }

}



