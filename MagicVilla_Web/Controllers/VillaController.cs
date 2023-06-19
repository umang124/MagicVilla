﻿using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Models;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexVilla()
        {
            List<Villa> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<Villa>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
