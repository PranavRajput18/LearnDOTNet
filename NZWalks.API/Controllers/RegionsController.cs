using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
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

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task <IActionResult> GetAll()
        {
            var regionsDomainModel = await  regionRepository.GetAllAsync();
           
            ///Map Domain To DTO
            var regions = mapper.Map<List<RegionDto>>(regionsDomainModel);

            return Ok(regions);
        }

        //Get Region by Id
        //GET : https://localhost:3000/api/regions/1
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            // var region = dbContext.Regions.Find(id);

            //LINQ Method
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var region = mapper.Map<RegionDto>(regionDomain);
            return Ok(region);

        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRequestDto)
        {
                //Map DTO to Domain model
                var regionDomainModel = mapper.Map<Region>(addRequestDto);
                //Add Region to the database
                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                var regionDto = mapper.Map<RegionDto>(regionDomainModel);
                return CreatedAtAction(nameof(GetById), new { Id = regionDto.Id }, regionDto);
           
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async  Task<IActionResult> Update([FromRoute]Guid id,[FromBody]UpdateRegionRequestDto updateRegionRequestDto)
        {
           
                //Map Domain to DTO
                var regionModel = mapper.Map<Region>(updateRegionRequestDto);

                regionModel = await regionRepository.UpdateAsync(id, updateRegionRequestDto);

                //check if exist
                if (regionModel is null)
                {
                    return NotFound();
                }

                //Map Domain Model to Dto
                var regionDto = mapper.Map<RegionDto>(regionModel);

                return Ok(regionDto);
           
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var regionModel = await regionRepository.DeleteAsync(id);
            if (regionModel is null)
            {
                return NotFound();               
            }
           
            //Map Domain Model to Dto
            var regionDto = mapper.Map<RegionDto>(regionModel);

            return Ok(regionDto);
        }
    }
}
