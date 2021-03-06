﻿using Microsoft.AspNetCore.Mvc;
using Com.Danliris.Service.Merchandiser.Lib;
using Com.Danliris.Service.Merchandiser.Lib.Services;
using Com.Danliris.Service.Merchandiser.Lib.Models;
using Com.Danliris.Service.Merchandiser.WebApi.Helpers;
using Com.Danliris.Service.Merchandiser.Lib.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Com.Danliris.Service.Merchandiser.WebApi.Controllers.v1.BasicControllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/efficiencies")]
    [Authorize]
    public class EfficienciesController : BasicController<MerchandiserDbContext, EfficiencyService, EfficiencyViewModel, Efficiency>
    {
        private static readonly string ApiVersion = "1.0";
        public EfficienciesController(EfficiencyService service) : base(service, ApiVersion)
        {
        }

        [HttpGet("quantity/{Quantity}")]
        public async Task<IActionResult> GetByQuantity([FromRoute] int Quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await Service.ReadModelByQuantity(Quantity);

            if (model == null)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                    .Fail();
                return NotFound(Result);
            }

            try
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok<Efficiency, EfficiencyViewModel>(model, Service.MapToViewModel);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}