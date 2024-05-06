using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class WalksController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IWalkRepository walkRepository;

    public WalksController(IMapper mapper, IWalkRepository walkRepository)
    {
        this.mapper = mapper;
        this.walkRepository = walkRepository;
    }

    // GET: /api/walks?filterBy=Name&filterQuery=Park
    [HttpGet]
    [MapToApiVersion("1.0")]
    public async Task<IActionResult> GetAll([FromQuery] string? filterBy = null, [FromQuery] string? filterQuery = null,
    [FromQuery] string? orderBy = null, [FromQuery] bool isAscending = true,
    [FromQuery] int pageNum = 1, [FromQuery] int pageSize = 1000)
    {
        var walksDto = mapper.Map<List<WalkDto>>(await walkRepository.GetAllAsync(filterBy, filterQuery, orderBy, isAscending,
        pageNum, pageSize));

        return Ok(walksDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walkDomain = await walkRepository.GetByIdAsync(id);
        var walkDto = mapper.Map<WalkDto>(walkDomain);
        return Ok(walkDto);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {
        var walkDomain = mapper.Map<Walk>(addWalkRequestDto);

        var walkDto = mapper.Map<WalkDto>(await walkRepository.CreateAsync(walkDomain));

        return CreatedAtAction(nameof(GetById), new { id = walkDomain.Id }, walkDto);
    }


    [HttpPut]
    [Route("{id:Guid}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
    {
        var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);
        Console.WriteLine("walkDomain", walkDomain);
        var walkDomainModel = await walkRepository.UpdateAsync(id, walkDomain);

        if (walkDomainModel == null)
        {
            return NotFound();
        }

        var walkDto = mapper.Map<WalkDto>(walkDomainModel);
        return Ok(walkDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {


        var walkDomain = await walkRepository.DeleteAsync(id);

        if (walkDomain == null)
        {
            return NotFound();
        }

        return Ok();
    }
}

