﻿namespace Assessment.Mappers
{
    using Assessment.Models;
    using AutoMapper;

    /// <summary>
    /// Customer Automapper Profile.
    /// </summary>
    public class CustomerProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerProfile"/> class.
        /// </summary>
        public CustomerProfile()
        {
            // The Created datetime would be better as a database trigger, but is good for now
            // Only update Created if destination is null
            this.CreateMap<CustomerInput, Customer>()
                .ForMember(dest => dest.Created, options => options.MapFrom((src, dest) => dest.Created ?? DateTime.Now))
                .ForMember(dest => dest.Updated, options => options.MapFrom(source => DateTime.Now));
        }
    }
}
