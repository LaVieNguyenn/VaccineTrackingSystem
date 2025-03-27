using AutoMapper;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.BLL.Enums;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Notification mapping
        CreateMap<Notification, NotificationResponseDTO>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child.FullName));

        // VaccinationRecord mapping
        CreateMap<VaccinationRecord, VaccinationRecordResponseDTO>()
            .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Child.FullName))
            .ForMember(dest => dest.VaccineName, opt => opt.MapFrom(src => src.Vaccine.VaccineName))
            .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.Staff.FullName));

        // Payment mapping
        CreateMap<Payment, PaymentResponseDTO>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => ((PaymentMethod)src.PaymentMethod).ToString()))
            .ForMember(dest => dest.PaymentStatusName, opt => opt.MapFrom(src => ((PaymentStatus)src.PaymentStatus).ToString()));

        // Feedback mapping
        CreateMap<Feedback, FeedbackResponseDTO>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName));
    }
} 