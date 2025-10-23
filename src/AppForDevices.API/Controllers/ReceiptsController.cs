using AppForDevices.Shared.RepairDTOs;

namespace AppForDevices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        //used to enable your controller to access to the database 
        private readonly ApplicationDbContext _context;
        //used to log any information when your system is running
        private readonly ILogger<ReceiptsController> _logger;

        public ReceiptsController(ApplicationDbContext context, ILogger<ReceiptsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(double),(int) HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(string),(int) HttpStatusCode.BadRequest)]
        //public async Task<IActionResult> ComputeDivision(double op1, double op2)
        //{
        //    //if (op2 == 0)
        //    //    return BadRequest("Op2 must be different from 0");
        //    //double result = op1 / op2;
        //    //return Ok(result);
        //    [HttpGet]

        //}
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReceiptDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReceipt(int id)
        {
            if (_context.Receipts == null)
            {
                _logger.LogError("Error: Receipts table does not exist");
                return NotFound();
            }
            var receipts = await _context.Receipts
                .Where(r => r.Id == id)
                    .Include(r => r.ReceiptItems) // join table ReceiptItems
                        .ThenInclude(ri => ri.Repair) //then join table Repair
                            .ThenInclude(repair => repair.Scale) // then join table Scale
                .Select(r => new ReceiptDetailDTO(
                    
                    r.Id,DateTime.Today,
                    r.ApplicationUser.UserName,
                    r.PaymentMethod,
                    r.DeliveryAddress,
                    r.CustomerNameSurname,
                    //new List<ReceiptItemDTO>()
                    r.ReceiptItems
                    .Select(ri => new ReceiptItemDTO(
                        ri.RepairId,
                        ri.Repair.Name,
                        ri.Repair.Cost,
                        ri.Repair.Scale.Name,
                        ri.Repair.Description,
                        ri.Model
                        )
                    ).ToList<ReceiptItemDTO>()
                    )
                ).FirstOrDefaultAsync();
            if (receipts == null)
            {
                _logger.LogError($"{DateTime.Now} error in table receipts");
                return NotFound();
            }
            return Ok(receipts);
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReceiptDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateReceipt(ReceiptForCreateDTO receiptForCreate)
        {
            if (receiptForCreate.ReceiptItems.Count == 0)
                ModelState.AddModelError("ReceiptItems", "Error! You must include at least one repair to be repaired");
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == receiptForCreate.customerUserName);
            if (user == null)
                ModelState.AddModelError("ReceiptApplicationUser", "Error! UserName is not registered");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            //Seleccionamos la lista de nombre de los reparos 
            var repairNames = receiptForCreate.ReceiptItems.Select(ri => ri.Name).ToList<string>();

            var repairs = _context.Repairs
                .Include(r=>r.Scale)
                // <-- repairNames son los reparos seleccionados para crear el recibo
            //seleccionamos lo que nos interesa
                //we use an anonymous type
                .Select(r => r)
                .ToList();
            //creamos el nuevo recibo con una lista vacía de Registros
            Receipt receipt = new Receipt(receiptForCreate.CustomerNameSurname,
                receiptForCreate.DeliveryAddress,
                receiptForCreate.PaymentMethod,
                new List<ReceiptItem>(),
                user,
                DateTime.Today);

           

            // aquí vamos añadiendo registros que estarán asociados al recibo creado anteriormente
            foreach (var item in receiptForCreate.ReceiptItems)
            {
                var repair = repairs.FirstOrDefault(r => r.Id == item.RepairId);
                if (repair == null)
                {
                    ModelState.AddModelError("ReceiptItemsForLoop", $"Error! Repair with Id '{item.RepairId}' is not available");
                } else
                {
                    receipt.ReceiptItems.Add(new ReceiptItem(repair, receipt, item.Model));
                }

            }
            //calculamos precio total
            receipt.TotalPrice = receipt.ReceiptItems.Sum(ri => ri.Repair.Cost);


            //if there repair does not exist
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            _context.Add(receipt);

            //try
            //{
                //we store in the DB both receipt and receiptItems
                await _context.SaveChangesAsync();
            ////} catch (Exception ex)
            ////{
            ////    _logger.LogError(ex.Message);
            ////    ModelState.AddModelError("Receipt", $"Error! There was an error while saving your receipt, please, try again later.");
            ////    return BadRequest(new ValidationProblemDetails(ModelState));
            ////}

            //it returns the receiptDetail
            var receiptDetail = new ReceiptDetailDTO(receipt.Id, DateTime.Today,receipt.ApplicationUser.UserName, receipt.PaymentMethod,
                 receipt.DeliveryAddress,
                receipt.CustomerNameSurname, new List<ReceiptItemDTO>() { });
            foreach (var item in receipt.ReceiptItems)
            {
                var ri = receipt.ReceiptItems.FirstOrDefault();
                receiptDetail.ReceiptItems.Add(new ReceiptItemDTO(item.Repair.Id, item.Repair.Name, item.Repair.Cost, item.Repair.Scale.Name, item.Repair.Description, item.Model));
            }

            return CreatedAtAction("GetReceipt", new { id = receipt.Id }, receiptDetail);
        }
    }
}
