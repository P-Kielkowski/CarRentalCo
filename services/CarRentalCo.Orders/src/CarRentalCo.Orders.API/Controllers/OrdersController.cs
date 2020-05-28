﻿using CarRentalCo.Common.Application.Handlers;
using CarRentalCo.Orders.API.Requests;
using CarRentalCo.Orders.Application.Orders.Dtos;
using CarRentalCo.Orders.Application.Orders.Features.CreateOrder;
using CarRentalCo.Orders.Application.Orders.Features.GetCustomerOrders;
using CarRentalCo.Orders.Application.Orders.Features.GetOrder;
using CarRentalCo.Orders.Application.Orders.Features.GetOrders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CarRentalCo.Orders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IQueryHandler<GetOrdersQuery, ICollection<OrderDto>> getOrdersQueryHandler;
        private readonly IQueryHandler<GetOrderQuery, OrderDto> getOrderQueryHandler;
        private readonly IQueryHandler<GetCustomerOrdersQuery, ICollection<OrderDto>> getCustomerOrdersQuery;
        private readonly ICommandHandler<CreateOrderCommand> createOrderCommandHandler;

        public OrdersController(IQueryHandler<GetOrdersQuery,ICollection<OrderDto>> getOrdersQueryHandler,
                IQueryHandler<GetOrderQuery, OrderDto> getOrderQueryHandler,
                IQueryHandler<GetCustomerOrdersQuery, ICollection<OrderDto>> getCustomerOrdersQuery,
                ICommandHandler<CreateOrderCommand> createOrderCommandHandler
            )
        {
            this.getOrdersQueryHandler = getOrdersQueryHandler;
            this.getOrderQueryHandler = getOrderQueryHandler;
            this.getCustomerOrdersQuery = getCustomerOrdersQuery;
            this.createOrderCommandHandler = createOrderCommandHandler;
        }

        /// <summary>
        /// Get orders
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Data</response>
        /// <response code="400"></response>
        /// <response code="404">If no order exists</response>
        [HttpGet]
        [Route("{pageNumber}/{pageSize}")]
        [ProducesResponseType(typeof(ICollection<OrderDto>), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetOrders([FromRoute] int pageNumber, [FromRoute] int pageSize)
        {
            var result = await getOrdersQueryHandler.HandleAsync(new GetOrdersQuery { PageNumber = pageNumber, PageSize = pageSize});

            if (result == null || result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Data</response>
        /// <response code="400"></response>
        /// <response code="404">If no order exists</response>
        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetOrder([FromRoute] Guid orderId)
        {
            var result = await getOrderQueryHandler.HandleAsync(new GetOrderQuery {OrderId = orderId });

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        /// <summary>
        /// Get order by id
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Data</response>
        /// <response code="400"></response>
        /// <response code="404">If no order exists</response>
        [HttpGet]
        [Route("customer/{customerId}")]
        [ProducesResponseType(typeof(ICollection<OrderDto>), StatusCodes.Status200OK)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetCustomerOrders([FromRoute] Guid customerId)
        {
            var result = await getCustomerOrdersQuery.HandleAsync(new GetCustomerOrdersQuery { CustomerId = customerId });

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Add order
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Created an order</response>
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetCustomerOrders([FromBody] CreateOrderRequest command)
        {
            //todo mapper
            await createOrderCommandHandler.HandleAsync(new CreateOrderCommand(command.OrderId, command.CustomerId,
                command.OrderCars.Select(x => new CreateOrderOrderCarModel(x.RentalCarId, x.RentalStartDate, x.RentalEndDate)).ToList()));

            return Ok(command.OrderId);
        }

    }
}