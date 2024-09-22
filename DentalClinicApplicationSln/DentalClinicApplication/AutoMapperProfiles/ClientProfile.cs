﻿using AutoMapper;
using DentalClinicApp.Models;
using DentalClinicApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.AutoMapperProfiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<ClientDTO, Client>()
                .ForMember(
                dest => dest.Gender,
                opts => opts.MapFrom(src => GetGenderEnum(src.Gender))
                );
            CreateMap<Client, ClientDTO>()
                .ForMember(
                dest => dest.Gender,
                opts => opts.MapFrom(
                    src => GetGenderString(src.Gender)
                    )
                );
        }

        private Gender GetGenderEnum(string gender)
        {
            return gender switch
            {
                "Male" => Gender.Male,
                "Female" => Gender.Female,
                _ => Gender.Undefinded
            };
        }

        private string GetGenderString(Gender gender)
        {
            return gender switch
            {
                Gender.Male => "Male",
                Gender.Female => "Female",
                _ => "Undefined"
            };
        }

        
    }
}
