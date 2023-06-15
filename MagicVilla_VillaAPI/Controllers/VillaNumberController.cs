using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTOs;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        protected APIResponse _response;
        public VillaNumberController(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVillaNumbers()
        {
            return Ok(await _dbContext.VillaNumber.ToListAsync());
        }

        [HttpGet("GetVillaNumber/{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVillaNumber(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villaNumber = await _dbContext.VillaNumber.FirstOrDefaultAsync(x => x.VillaId == id);
            if (villaNumber == null)
            {
                return NotFound();
            }
            return Ok(villaNumber);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber([FromBody] VillaNumberDTO villaNumber)
        {
            if (villaNumber.VillaNo == 0)
            {
                ModelState.AddModelError("CustomError", "VillaNo cannot be zero");
                return BadRequest(ModelState);
            }

            if (!_dbContext.Villas.Any(x => x.Id == villaNumber.VillaId))
            {
                ModelState.AddModelError("CustomError", "VillaId doesnot exists");
                return BadRequest(ModelState);
            }
            if (_dbContext.VillaNumber.Any(x => x.VillaNo == villaNumber.VillaNo))
            {
                ModelState.AddModelError("CustomError", "VillaNo already exists");
                return BadRequest(ModelState);
            }
            var model = _mapper.Map<VillaNumber>(villaNumber);
            await _dbContext.VillaNumber.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return CreatedAtRoute("GetVillaNumber", new { id = model.VillaId }, model);
        }
        [HttpDelete("DeleteVillaNumber/{id:int}")]
        public async Task<IActionResult> DeleteVillaNumber(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villaNumber = await _dbContext.VillaNumber.FirstOrDefaultAsync(x => x.VillaNo == id);
            if (villaNumber == null)
            {
                return NotFound();
            }
                
            _dbContext.VillaNumber.Remove(villaNumber);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("UpdateVillaNumber/{id:int}")]
        public async Task<IActionResult> UpdateVillaNumber([FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var villaNumber = await _dbContext.VillaNumber.AsNoTracking().FirstOrDefaultAsync(x => x.VillaNo == villaNumberUpdateDTO.VillaNo);
            if (villaNumber == null)
            {
                return NotFound();
            }

            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
            _dbContext.VillaNumber.Update(model);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
