﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTOs;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public VillaAPIController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetVillas()
        {           
            return Ok(await _dbContext.Villas.ToListAsync());
        }

        [HttpGet("GetVilla/{id:int}", Name = "GetVilla")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetVilla(int id)
        {
            if (id == 0)
            {               
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            if (villaCreateDTO == null)
            {
                return BadRequest();
            }
            if (await _dbContext.Villas.AnyAsync(x => x.Name.ToLower() == villaCreateDTO.Name.ToLower()))
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
            }
            Villa model = new()
            {
                Amenity = villaCreateDTO.Amenity,
                Details = villaCreateDTO.Details,
                ImageUrl = villaCreateDTO.ImageUrl,
                Name = villaCreateDTO.Name,
                Occupancy = villaCreateDTO.Occupancy,
                Rate = villaCreateDTO.Rate,
                Sqft = villaCreateDTO.Sqft
            };

            await _dbContext.Villas.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new {id = model.Id}, model);
        }

        [HttpDelete("DeleteVilla/{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
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
                return BadRequest();
            }
            var getVilla = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == villaUpdateDTO.Id);
            if (getVilla == null)
            {
                return NotFound();
            }

            Villa villa = new()
            {
                Amenity = villaUpdateDTO.Amenity,
                Details = villaUpdateDTO.Details,
                Id = villaUpdateDTO.Id,
                ImageUrl = villaUpdateDTO.ImageUrl,
                Name = villaUpdateDTO.Name,
                Occupancy = villaUpdateDTO.Occupancy,
                Rate = villaUpdateDTO.Rate,
                Sqft = villaUpdateDTO.Sqft
            };
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
                return BadRequest();
            }

            var villa = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null)
            {
                return BadRequest();
            }

            VillaUpdateDTO villaDTO = new VillaUpdateDTO()
            {
                Amenity = villa.Amenity ?? "",
                Details = villa.Details,
                Id = villa.Id,
                ImageUrl = villa.ImageUrl ?? "",
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft
            };

            patchUpdateDTO.ApplyTo(villaDTO, ModelState);

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft
            };

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
