using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {

        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // Get all regions
        // GET: http://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //  Get Data From Database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();
            //  Map Domain Models To DTOs

            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            // var regionsDto = new List<RegionDto>();
            // foreach (var regionDomain in regionsDomain)
            // {
            //     regionsDto.Add(new RegionDto()
            //     {
            //         Id = regionDomain.Id,
            //         Code = regionDomain.Code,
            //         Name = regionDomain.Name,
            //         RegionImageUrl = regionDomain.RegionImageUrl
            //     });
            // }
            // Return DTOs to Client
            return Ok(regionsDto);
        }

        // Get single regions
        // GET: http://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            // Only take primary key
            // var region = dbContext.Regions.Find(id); 
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionDomain);
            // var regionDto = new RegionDto()
            // {
            //     Id = regionDomain.Id,
            //     Code = regionDomain.Code,
            //     Name = regionDomain.Name,
            //     RegionImageUrl = regionDomain.RegionImageUrl

            // };

            return Ok(regionDto);
        }

        // Post single region
        // Post: http://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or convert Dto to Domain Model
            // var regionDomainModel = new Region
            // {
            //     Code = addRegionRequestDto.Code,
            //     Name = addRegionRequestDto.Name,
            //     RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            // };
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);


            // Use Domain Model to Create Region
            await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain Model Back to Dto
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            // var regionDto = new RegionDto
            // {
            //     Id = regionDomainModel.Id,
            //     Name = regionDomainModel.Name,
            //     Code = regionDomainModel.Code,
            //     RegionImageUrl = regionDomainModel.RegionImageUrl,
            // };

            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionDto);
        }


        // Update a single region
        // Put: http://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            var regionDomain = mapper.Map<Region>(updateRegionRequestDto);
            // var regionDomain = new Region()
            // {
            //     Code = "",
            //     Name = updateRegionRequestDto.Name,
            //     RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
            // };


            var regionDomainModel = await regionRepository.UpdateAsync(id, regionDomain);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionDomain);
            // var regionDto = new RegionDto()
            // {
            //     Id = regionDomainModel.Id,
            //     Code = regionDomainModel.Code,
            //     Name = regionDomainModel.Name,
            //     RegionImageUrl = regionDomainModel.RegionImageUrl,
            // };

            return Ok(regionDto);
        }


        // Delete a region from database
        // Delete: http://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
