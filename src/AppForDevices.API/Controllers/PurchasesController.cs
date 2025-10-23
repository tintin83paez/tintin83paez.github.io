using AppForDevices.Shared.PurchaseDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForDevices.Shared.PurchaseDTOs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using AppForDevices.Shared.DeviceDTOs;


namespace AppForDevices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //GET: API:Purchases
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            if (_context.Purchases == null)
            {
                _logger.LogError("Error: Purchases table does not exist");
                return NotFound();
            }

            var purchasedetaildto = await _context.Purchases
            .Where(purchase => purchase.Id == id)
                .Include(purchase => purchase.PurchaseItems)
                    .ThenInclude(purchaseItem => purchaseItem.Device)
                        .ThenInclude(device => device.Model)
            .Select(purchase => new PurchaseDetailDTO(purchase.Id, purchase.DeliveryAddress, purchase.PurchaseItems
                .Select(pi => new PurchaseItemDTO(pi.Device.Id, pi.Device.Name, pi.Device.priceForPurchase, pi.Device.Model.NameModel, pi.Quantity)).ToList<PurchaseItemDTO>(),
             purchase.CustomerUserName, purchase.CustomerUserSurname, purchase.PaymentMethod, purchase.PurchaseDate, purchase.TotalPrice, purchase.TotalQuantity))
            .FirstOrDefaultAsync();

            if(purchasedetaildto == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }
            return Ok(purchasedetaildto);
        }

        //POST: API/Purchases
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseforcreate)
        {
            if (_context.Purchases == null)
            {
                _logger.LogError("Error: Purchases table does not exist");
                return Problem("Entity set 'ApplicationDBContext.Purchases' is null");
            }

            if (purchaseforcreate.PurchaseItems.Count == 0)
            {
                ModelState.AddModelError("PurchaseItems", "Error! You must include at least one device to be purchased");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == purchaseforcreate.UserName);
            if (user == null)
            {
                ModelState.AddModelError("PurchaseApplicationUser", "Error! UserName is not registered");
            }

            Purchase purchase = new Purchase(purchaseforcreate.DeliveryAddress, purchaseforcreate.UserName, purchaseforcreate.NameSurname, DateTime.Now, purchaseforcreate.TotalPrice, purchaseforcreate.TotalQuantity, new List<PurchaseItem>(), purchaseforcreate.PaymentMethod, user);

            //Check if we have enough devices to be bought in the database
            Device device;
            foreach (var item in purchaseforcreate.PurchaseItems)
            {
                device = await _context.Devices.FindAsync(item.DeviceId);
                if (device == null)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! Device named {item.Name} with Id {item.DeviceId} does not exist in the database");
                }
                else
                {
                    if (device.quantityForPurchase < item.Quantity)
                    {
                        ModelState.AddModelError("PurchaseItems", $"Error! Device named {item.Name} only has {device.quantityForPurchase} units available but {item.Quantity} were selected");
                    }
                    else
                    {
                        device.quantityForPurchase -= item.Quantity;
                        purchase.PurchaseItems.Add(new PurchaseItem(device, item.Quantity, purchase));
                    }
                }
            }
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            var purchaseDetail = new PurchaseDetailDTO(purchase.Id, purchase.DeliveryAddress, purchaseforcreate.PurchaseItems, purchase.CustomerUserName, purchase.CustomerUserSurname, purchase.PaymentMethod, purchase.PurchaseDate, purchase.TotalPrice, purchase.TotalQuantity);

            return CreatedAtAction("GetPurchase", new { Id = purchase.Id }, purchaseDetail);
        }
    }

}
