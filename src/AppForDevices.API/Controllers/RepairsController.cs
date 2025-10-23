using AppForDevices.Shared.RepairDTOs;

namespace AppForDevices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairsController : ControllerBase
    {
        //used to enable your controller to access to the database 
        private readonly ApplicationDbContext _context;
        //used to log any information when your system is running
        private readonly ILogger<RepairsController> _logger;

        public RepairsController(ApplicationDbContext context, ILogger<RepairsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RepairDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRepairsForMaintenance(string? repairName, string? repairScale, float ? cost_less_or_equal)
        {
            IList<RepairDTO> selectRepairs = await _context.Repairs
                //join table Repair and table Scale
                .Include(r => r.Scale)

                //join tables Repair and ReceiptItem and then Receipt
                .Include(r => r.ReceiptItems).ThenInclude(ri => ri.Receipt)

                .Where(repair => (repairName == null || repair.Name.Contains(repairName))
                && (repairScale == null || repair.Scale.Name.Equals(repairScale))
                && (cost_less_or_equal == null || repair.Cost <= cost_less_or_equal))
                .OrderBy(r => r.Name)

                .Select(r => new RepairDTO(
                    r.Id, r.Name, r.Cost, r.Scale.Name, r.Description)
                ).ToListAsync();
            return Ok(selectRepairs);
        }
    }
}
