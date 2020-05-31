﻿using CarRentalCo.Administration.API.Requests;
using CarRentalCo.Administration.Application.Companies.Dtos;
using CarRentalCo.Administration.Application.Companies.Features.CreateCompany;
using CarRentalCo.Administration.Application.Companies.Features.GetCompany;
using CarRentalCo.Administration.Domain.Companies;
using CarRentalCo.Common.Application.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace CarRentalCo.Administration.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICommandHandler<CreateCompanyCommand> createCompanyHandler;
        private readonly IQueryHandler<GetCompanyQuery, CompanyDto> getCompanyHandler;

        public CompaniesController(ICommandHandler<CreateCompanyCommand> createCompanyHandler,
            IQueryHandler<GetCompanyQuery, CompanyDto> getCompanyHandler)
        {
            this.createCompanyHandler = createCompanyHandler;
            this.getCompanyHandler = getCompanyHandler;
        }

        /// <summary>
        /// Get company by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Data</response>
        /// <response code="400"></response>
        /// <response code="404">If no company exists</response>
        [HttpGet]
        [Route("{companyId}")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetCompany([FromRoute] Guid companyId)
        {
            var result = await getCompanyHandler.HandleAsync(new GetCompanyQuery { Id = new Domain.Companies.CompanyId(companyId) });

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Add company
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Data</response>
        /// <response code="400"></response>
        [HttpPost]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> AddCompany([FromBody] CreateCompanyRequest request)
        {
            await createCompanyHandler.HandleAsync(new CreateCompanyCommand(new CompanyId(request.CompanyId),
                new Domain.Owners.OwnerId(request.OwnerId), request.Name, request.Email, request.Phone));

            return Ok(request.CompanyId);
        }
    }
}