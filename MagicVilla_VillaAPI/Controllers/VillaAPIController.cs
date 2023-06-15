using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTOs;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaAPIController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetVillas()
        {
            _response.Result = await _dbContext.Villas.ToListAsync();
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("GetVilla/{id:int}", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetVilla(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            var villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = villa;
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            if (villaCreateDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            if (await _dbContext.Villas.AnyAsync(x => x.Name.ToLower() == villaCreateDTO.Name.ToLower()))
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(_response);
            }
            Villa model = _mapper.Map<Villa>(villaCreateDTO);

            await _dbContext.Villas.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            _response.StatusCode = HttpStatusCode.Created;
            _response.Result = model;
            _response.IsSuccess = true;
            return CreatedAtRoute("GetVilla", new {id = model.Id}, _response);
        }

        [HttpDelete("DeleteVilla/{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }
            _dbContext.Villas.Remove(villa);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("UpdateVilla/{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateVilla([FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            if (villaUpdateDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var getVilla = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == villaUpdateDTO.Id);
            if (getVilla == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            Villa villa = _mapper.Map<Villa>(villaUpdateDTO);
            _dbContext.Villas.Update(villa);
            await _dbContext.SaveChangesAsync();

            return NoContent();     
        }

        [HttpPatch("UpdatePartialVilla/{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchUpdateDTO)
        {
            if (id == 0 || patchUpdateDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest();
            }

            var villa = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest();
            }

            VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);

            patchUpdateDTO.ApplyTo(villaUpdateDTO, ModelState);

            Villa model = _mapper.Map<Villa>(villaUpdateDTO);

            _dbContext.Villas.Update(model);
            await _dbContext.SaveChangesAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
