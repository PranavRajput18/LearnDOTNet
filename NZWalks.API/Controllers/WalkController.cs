using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
    {

        private readonly IMapper mapper;

        private readonly IWalkRepository walkRepository;

        public WalkController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
          
                //Map Dto to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                //Map Domain to DTO
                var walks = mapper.Map<WalkDto>(walkDomainModel);
                return Ok(walks);
        }
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var wallkDomainModel = await walkRepository.GetAll();

            //Map Domain to DTO
            var walks = mapper.Map<List<WalkDto>>(wallkDomainModel);
            return Ok(walks);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            //Map Domain to Dto
            var walk = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walk);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody]UpdateWalkRequestDto updateWalkRequestDto)
        {
                //Map DTO to Domain
                var walk = mapper.Map<Walk>(updateWalkRequestDto);

                var updateWalkDomainModel = await walkRepository.UpdateAsync(walk, id);
                if (updateWalkDomainModel == null) {
                    return NotFound();
                }
                //Map Domain to DTO
                var walkDto = mapper.Map<WalkDto>(updateWalkDomainModel);
                return Ok(walkDto);
        
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if (walkDomainModel == null) 
            { 
                return NotFound(); 
            }
            //Map Domain to DTO
            var walk = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walk);
        }
    }
}
