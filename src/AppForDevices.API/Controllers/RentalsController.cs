using AppForDevices.Shared.RentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using AppForDevices.Shared.DeviceDTOs;
using AppForDevices.API.Models;
using System.Linq;


namespace AppForDevices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(ApplicationDbContext context, ILogger<RentalsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //GET: API:Rentals
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRental(int id)
        {
            if (_context.Rentals == null)
            {
                _logger.LogError("Error: Rentals table does not exist");
                return NotFound();
            }

            var rental = await _context.Rentals
             .Where(r => r.Id == id)
                 .Include(r => r.RentDevices) //join table RentalItems
                    .ThenInclude(ri => ri.Device) //then join table Movies
                        .ThenInclude(device => device.Model) //then join table Model
             .Select(r => new RentalDetailDTO(r.Id, r.RentalDate, r.Name,
                    r.Surname, r.DeliveryAddress,
                    (Shared.PaymentMethodTypes)r.PaymentMethod,
                    r.RentalDateFrom, r.RentalDateTo,
                    r.RentDevices
                        .Select(ri => new RentalItemDTO(ri.Device.Model.NameModel,
                                ri.Device.Quality, ri.Device.priceForRent,
                                ri.Device.Id, ri.Quantity)).ToList<RentalItemDTO>()))
             .FirstOrDefaultAsync();


            if (rental == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(rental);
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateRental(RentalForCreateDTO rentalForCreate)
        {
            //any validation defined in PurchaseForCreate is checked before running the method so they don't have to be checked again
            if (rentalForCreate.RentalDateFrom <= DateTime.Today)
                ModelState.AddModelError("RentalDateFrom", "Error! Your rental date must start later than today");

            if (rentalForCreate.RentalDateFrom >= rentalForCreate.RentalDateTo)
                ModelState.AddModelError("RentalDateFrom&RentalDateTo", "Error! Your rental must end later than it starts");

            if (rentalForCreate.RentalItems.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! You must include at least one device to be rented");

            // if (!_context.ApplicationUsers.Any(au=>au.UserName==rentalForCreate.CustomerUserName))
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == rentalForCreate.Name);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var deviceNames = rentalForCreate.RentalItems.Select(ri => ri.DeviceId).ToList<int>();

            var devices = _context.Devices.Include(d => d.RentedDevices)
                .ThenInclude(ri => ri.Rental)
                .Where(d => deviceNames.Contains(d.Id))

                //we use an anonymous type https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
                .Select(d => new {
                    d.Id,
                    d.Name,
                    d.quantityForRent,
                    d.priceForRent,
                    //we count the number of rentalItems that are within the rental period
                    NumberOfRentedItems = d.RentedDevices.Count(ri => ri.Rental.RentalDateFrom <= rentalForCreate.RentalDateTo
                            && ri.Rental.RentalDateTo >= rentalForCreate.RentalDateFrom)
                })
                .ToList();

            Rental rental = new Rental(rentalForCreate.Name, rentalForCreate.Surname,
                rentalForCreate.DeliveryAddress, DateTime.Now, rentalForCreate.RentalDateFrom, rentalForCreate.RentalDateTo, new List<RentDevice>(), rentalForCreate.PaymentMethod, user);


            rental.TotalPrice = 0;
            var numDays = (rental.RentalDateTo - rental.RentalDateFrom).TotalDays;


            foreach (var item in rentalForCreate.RentalItems)
            {
                var device = devices.FirstOrDefault(d => d.Name == item.Name);
                //we must check that there is enough quantity to be rented in the database
                if ((device == null) || (device.NumberOfRentedItems >= device.quantityForRent))
                {
                    ModelState.AddModelError("RentalItems", $"Error! Device is not available for being rented for these days");
                }
                else
                {
                    // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                    rental.RentDevices.Add(new RentDevice(device.Id, rental, device.priceForRent));
                    item.PriceForRental = device.priceForRent;
                }
            }
            rental.TotalPrice = rental.RentDevices.Sum(ri => ri.Price * numDays); // En el caso de RentDevice es similar priceForRent, estamos ya dentro de la clase de rentas por lo que solo hay 1 precio


            //if there is any problem because of the available quantity of movies or because the movie does not exist
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(rental);

            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);

            }

            //it returns rentalDetail
            var rentalDetail = new RentalDetailDTO(rental.Id, rental.RentalDate,
                rental.Name, rental.Surname,
                rental.DeliveryAddress, rentalForCreate.PaymentMethod,
                rental.RentalDateFrom, rental.RentalDateTo,
                rentalForCreate.RentalItems);

            return CreatedAtAction("GetRental", new { id = rental.Id }, rentalDetail);
        }
    }
}