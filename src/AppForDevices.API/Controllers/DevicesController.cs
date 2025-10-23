using AppForDevices.Shared.DeviceDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using System.Reflection;

namespace AppForDevices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(ApplicationDbContext context, ILogger<DevicesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*[HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        {
            if (op2 == 0)
            {
                _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");
                return BadRequest("op2 must be different from 0");
            }
            decimal result = decimal.Round(op1 / op2, 2);
            return Ok(result);
        }*/

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DeviceForRentalDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForRental(string? model, double? priceForRental, string? color)

        {
            IList<DeviceForRentalDTO> devices = await _context.Devices
                .Include(d => d.Model)
                .Where(d => d.quantityForRent > 0 
                            && (model == null || d.Model.NameModel.Contains(model))
                            // Si es un string es d.Name.Contains(name). el equals con un objeto, int double con <>==
                            && (priceForRental == null || d.priceForRent == priceForRental)
                            && (color == null || d.Color.Contains(color)))
                .OrderBy(d => d.Name)
                .Select(d => new DeviceForRentalDTO(d.Id, d.Name, d.Model.NameModel,
                            d.Brand, d.Color, d.quantityForRent, d.priceForRent, d.Quality))  // Mapeo con QualityType
                .ToListAsync();
               return Ok(devices);
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DeviceForPurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForPurchase(string? name, string? color, string? model)
        { 
            IList<DeviceForPurchaseDTO> devices = await _context.Devices
                .Include(d=>d.Model)
                .Include(d=>d.PurchaseItems).ThenInclude(pi=>pi.Purchase)
                .Where(d=>(d.Name.Contains(name) || (name == null))
                        && ((model == null)||(d.Model.NameModel.Equals(model)))
                        && ((d.Color == color) || (color == null))
                        && (d.quantityForPurchase > 0))
                .OrderBy(d => d.Name)
                .Select(d=>new DeviceForPurchaseDTO(d.Id, d.Name, d.Brand, d.Color, d.priceForPurchase, d.Model.NameModel))
                .ToListAsync();

            return Ok(devices);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DeviceForLeavingReviewDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetDevicesForLeavingReview(string? brand, int? year, string? name)
        {
            try
            {
                // Fetch devices for review
                IList<DeviceForLeavingReviewDTO> devices = await _context.Devices
                    .Where(d =>
                        (string.IsNullOrEmpty(brand) || d.Brand.Equals(brand)) &&
                        (!year.HasValue || d.Year == year) &&
                        (d.Name.Contains(name) || (name == null))
                    )
                    .OrderBy(d => d.Name)
                    .Select(d => new DeviceForLeavingReviewDTO(
                        d.Id,
                        d.Name,
                        d.Brand,
                        d.Model.NameModel,
                        d.Description,
                        d.Color,
                        d.Year
                    ))
                    .ToListAsync();

                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting devices for leaving review.");
                return BadRequest("An error occurred while fetching the devices. Please try again later.");
            }
        }
    }


    }

