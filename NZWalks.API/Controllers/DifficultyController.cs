using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyController : ControllerBase
    {
        private readonly IDifficultyRepository difficultyRepository;
        private readonly IMapper mapper;

        public DifficultyController(IDifficultyRepository difficultyRepository,IMapper mapper)
        {
            this.difficultyRepository = difficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
       public async Task<IActionResult> GetAll() {

            var difficultiesDomainModel = await difficultyRepository.GetAllAsync();
            //Map Domain to DTO
            var difficulties =  mapper.Map<List<DifficultyDto>>(difficultiesDomainModel);
            return Ok(difficulties); 
       }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var difficultiesDomainModel = await difficultyRepository.GetByIdAsync(id);

            //Map Domain to DTO
            var difficulties = mapper.Map<DifficultyDto>(difficultiesDomainModel);
            return Ok(difficulties);

        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AddRequestDifficultiesDto addRequestDifficulties)
        {
            //Map DTO to Domain
            var difficultiesDomain = mapper.Map<Difficulty>(addRequestDifficulties);

            difficultiesDomain = await difficultyRepository.CreateAsync(difficultiesDomain);

            //Map Domain to DTO 
            var difficulties = mapper.Map<DifficultyDto>(difficultiesDomain);
            return Ok(difficulties);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromBody]UpdateDifficultiesDto updateDifficultiesDto,[FromRoute]Guid id)
        {
            //Map DTO to Domain
            var difficultiesDomain = mapper.Map<Difficulty>(updateDifficultiesDto);

            difficultiesDomain = await difficultyRepository.UpdateAsync(difficultiesDomain,id);

            //Map Domain to DTO
            var difficultiesDto = mapper.Map<DifficultyDto>(difficultiesDomain);
            return Ok(difficultiesDto);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var diffcultiesDomain = await difficultyRepository.DeleteAsync(id);

            //Map Domain to DTO
            var diffcultiesDto = mapper.Map<DifficultyDto>(diffcultiesDomain);
            return Ok(diffcultiesDto);
        }

    }
}
