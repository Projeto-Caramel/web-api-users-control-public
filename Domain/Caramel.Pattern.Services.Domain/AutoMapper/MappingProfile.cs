﻿using AutoMapper;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Adopters;
using Caramel.Pattern.Services.Domain.Entities.DTOs.Request.Partners;
using Caramel.Pattern.Services.Domain.Entities.Models.Adopters;
using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using System.Diagnostics.CodeAnalysis;

namespace Caramel.Pattern.Services.Domain.AutoMapper
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Partners

            CreateMap<PartnerRegistrationRequest, Partner>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.Cellphone, opt => opt.MapFrom(src => src.Cellphone))
                .ForMember(x => x.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(x => x.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(x => x.MaxCapacity, opt => opt.MapFrom(src => src.MaxCapacity));

            CreateMap<PartnerUpdateRequest, Partner>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(x => x.Cellphone, opt => opt.MapFrom(src => src.Cellphone))
                .ForMember(x => x.CNPJ, opt => opt.MapFrom(src => src.CNPJ))
                .ForMember(x => x.AdoptionRate, opt => opt.MapFrom(src => src.AdoptionRate))
                .ForMember(x => x.PIX, opt => opt.MapFrom(src => src.PIX))
                .ForMember(x => x.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(x => x.Instagram, opt => opt.MapFrom(src => src.Instagram))
                .ForMember(x => x.Facebook, opt => opt.MapFrom(src => src.Facebook));
            
            CreateMap<PartnerUpdatePasswordRequest, Partner>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(x => x.Cellphone, opt => opt.MapFrom(src => src.Cellphone))
                .ForMember(x => x.CNPJ, opt => opt.MapFrom(src => src.CNPJ))
                .ForMember(x => x.AdoptionRate, opt => opt.MapFrom(src => src.AdoptionRate))
                .ForMember(x => x.PIX, opt => opt.MapFrom(src => src.PIX))
                .ForMember(x => x.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(x => x.Instagram, opt => opt.MapFrom(src => src.Instagram))
                .ForMember(x => x.Facebook, opt => opt.MapFrom(src => src.Facebook))
                .ForMember(x => x.ProfileImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl));

            #endregion

            #region Adopters

            CreateMap<AdopterRegistrationRequest, Adopter>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.Birthday, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(x => x.ResidencyType, opt => opt.MapFrom(src => src.ResidencyType))
                .ForMember(x => x.Lifestyle, opt => opt.MapFrom(src => src.Lifestyle))
                .ForMember(x => x.PetExperience, opt => opt.MapFrom(src => src.PetExperience))
                .ForMember(x => x.HasChildren, opt => opt.MapFrom(src => src.HasChildren))
                .ForMember(x => x.FinancialSituation, opt => opt.MapFrom(src => src.FinancialSituation))
                .ForMember(x => x.FreeTime, opt => opt.MapFrom(src => src.FreeTime));

            CreateMap<AdopterUpdateRequest, Adopter>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Birthday, opt => opt.MapFrom(src => src.Birthday))
                .ForMember(x => x.ResidencyType, opt => opt.MapFrom(src => src.ResidencyType))
                .ForMember(x => x.Lifestyle, opt => opt.MapFrom(src => src.Lifestyle))
                .ForMember(x => x.PetExperience, opt => opt.MapFrom(src => src.PetExperience))
                .ForMember(x => x.HasChildren, opt => opt.MapFrom(src => src.HasChildren))
                .ForMember(x => x.FinancialSituation, opt => opt.MapFrom(src => src.FinancialSituation))
                .ForMember(x => x.FreeTime, opt => opt.MapFrom(src => src.FreeTime));

            CreateMap<AdopterUpdatePasswordRequest, Adopter>()
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.NewPassword))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email));
            #endregion
        }
    }
}
