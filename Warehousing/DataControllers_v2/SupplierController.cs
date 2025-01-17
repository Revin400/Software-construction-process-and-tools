using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehousing.DataServices_v2;

namespace Warehousing.DataControllers_v2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SupplierController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }
       

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_supplierService.GetSuppliers());
        }

        [HttpGet("{id}")]
        public IActionResult GetSuppliersById(int id)
        {
            var supplier = _supplierService.GetSupplierById(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        [HttpPost]
        public IActionResult Createsupplier([FromBody] Supplier supplier)
        {
            var suppliers = _supplierService.GetSuppliers();
            if(suppliers.Any(s => s.Code == supplier.Code))
            {
                return BadRequest("Supplier already exists");
            }

            if(suppliers.Any(s => s.Code == supplier.Code && s.Id != supplier.Id))
            {
                return BadRequest("Code already exists");
            }

            if (!supplier.Reference.Contains("-SUP"))
            {
                return BadRequest("Reference must have this format 'CountryCode-SUP(reference code)'");
            }

            if (suppliers.Any(s => s.Reference == supplier.Reference && s.Id != supplier.Id))
            {
                return BadRequest("Reference Already Exists");
            }

            _supplierService.AddSupplier(supplier);
            return CreatedAtAction(nameof(GetSuppliersById), new { id = supplier.Id }, supplier);
        }

        [HttpPut("{id}")]
        public IActionResult Updatesupplier([FromBody] Supplier supplier)
        {
            var suppliers = _supplierService.GetSuppliers();
            if (!suppliers.Any(s => s.Id == supplier.Id))
            {
                return NotFound();
            }

            if(suppliers.Any(s => s.Code == supplier.Code && s.Id != supplier.Id))
            {
                return BadRequest("Code already exists");
            }

            if (!supplier.Reference.Contains("-SUP") || supplier.Reference.Length != 7)
            {
                return BadRequest("Reference must have this format 'CountryCode-SUP(reference code)'");
            }

            if (suppliers.Any(s => s.Reference == supplier.Reference && s.Id != supplier.Id))
            {
                return BadRequest("Reference Already Exists");
            }

            _supplierService.UpdateSupplier(supplier);
            return Ok(supplier);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletesupplier(int id)
        {          
            _supplierService.DeleteSupplier(id);
            return Ok();    
        }
    }
}