using AutoMapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.AutoMapperProfiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<AppointmentDTO, Appointment>()
                .ForMember(dest => dest.Duration,
                opts => opts.MapFrom(src => src.EndDate - src.StartDate));
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.EndDate,
                opts => opts.MapFrom(src => src.StartDate + src.Duration));
        }
    }
}
