﻿using CarRentalCo.Orders.Application.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalCo.Orders.Application.Orders.Clients
{
    public interface IRentalCarClient
    {
        Task<ICollection<RentalCarDto>> GetByIdsAsync(Guid[] Ids);
        Task<RentalCarDto> GetByIdAsync(Guid Id);
    }
}