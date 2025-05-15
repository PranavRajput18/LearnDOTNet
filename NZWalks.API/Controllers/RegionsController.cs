using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDomainModel = dbContext.Regions.ToList();
            Console.WriteLine("Regions Count: " + regionsDomainModel.Count);
            Console.WriteLine("Regions: " + regionsDomainModel);
            var regions = new List<RegionDto>();
            foreach (var region in regionsDomainModel)
            {
                regions.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            return Ok(regions);
        }

        //Get Region by Id
        //GET : https://localhost:3000/api/regions/1
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            // var region = dbContext.Regions.Find(id);

            //LINQ Method
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var region = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(region);

        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRequestDto)
        {
            //Create Domain model
            var regionDomainModel = new Region()
            {
                Code = addRequestDto.Code,
                Name = addRequestDto.Name,
                RegionImageUrl = addRequestDto.RegionImageUrl
            };
            //Add Region to the database
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            //Map to Dto
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById),new {Id = regionDto.Id},regionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute]Guid id,[FromBody]UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionModel = dbContext.Regions.Find(id);
            if (regionModel is null)
            {   
                return NotFound();
            }
            //Map Dto to Domain Model
            regionModel.Code = updateRegionRequestDto.Code;
            regionModel.Name = updateRegionRequestDto.Name;
            regionModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            //Map Domain Model to Dto
            var regionDto = new RegionDto
            {
                Id = regionModel.Id,
                Code = regionModel.Code,
                Name = regionModel.Name,
                RegionImageUrl = regionModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute]Guid id)
        {
            var regionModel = dbContext.Regions.Find(id);
            if (regionModel is null)
            {
                return NotFound();               
            }
             dbContext.Regions.Remove(regionModel);
            dbContext.SaveChanges();
            //Map Domain Model to Dto

            var regionDto = new RegionDto
            {
                Id = regionModel.Id,
                Code = regionModel.Code,
                Name = regionModel.Name,
                RegionImageUrl = regionModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
