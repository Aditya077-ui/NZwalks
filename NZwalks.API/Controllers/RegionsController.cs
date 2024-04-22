using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Models.Domain;
using NZwalks.API.Models.DTO;
using System.ComponentModel;

namespace NZwalks.API.Controllers
{
    //https://localhost:portnumber/api/regions   
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZwalksDbContext _dbContext;
        public RegionsController(NZwalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET ALL REGIONS
        //GET : https://localhost:portnumber/api/regions  
        [HttpGet]
        public IActionResult GetAll()
        {
            //get data from database - domain models
            var RegionsModel = _dbContext.Regions.ToList();

            //maps domain models into DTOs
            var RegionDTOs = new List<RegionDto>();
            foreach (var RegionModel in RegionsModel)
            {
                RegionDTOs.Add(new RegionDto()
                {
                    Id = RegionModel.Id,
                    Name = RegionModel.Name,
                    Code = RegionModel.Code,
                    RegionImageURl = RegionModel.RegionImageURl,
                });
            }

            //return DTOs
            return Ok(RegionDTOs);
        }

        //GET SINGLE REGION BY ID
        // GET : https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //get region domain model from database
            var RegionModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (RegionModel == null)
            {
                return NotFound();
            }

            //maps region domain model to Region DTos

            var Regiondto = new RegionDto
            {
                Id = RegionModel.Id,
                Name = RegionModel.Name,
                Code = RegionModel.Code,
                RegionImageURl = RegionModel.RegionImageURl,
            };

            //return DTO
            return Ok(Regiondto);
        }



        //POST TO CREATE NEW REGION
        //POST:https://localhost:portnumber/api/regions  
        [HttpPost]
        public IActionResult Create([FromBody] AddREgionRequestDTo addREgionRequestDTo)
        {
            //map RegionDTO to domain model
            var RegionDomainModel = new Region
            {
                Code = addREgionRequestDTo.Code,
                Name = addREgionRequestDTo.Name,
                RegionImageURl = addREgionRequestDTo.RegionImageURl
            };

            //use domain model to create Region(adding to database)
            _dbContext.Regions.Add(RegionDomainModel);
            _dbContext.SaveChanges();

            //map domain model back to DTO
            var RegionDTO = new RegionDto
            {
                Id = RegionDomainModel.Id,
                Name = RegionDomainModel.Name,
                Code = RegionDomainModel.Code,
                RegionImageURl = RegionDomainModel.RegionImageURl
            };

            return CreatedAtAction(nameof(GetById), new {id =  RegionDTO.Id},RegionDTO);

        }



        //update Region
        //PUT : https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //check if Region exists
            var RegionDomainModel = _dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if (RegionDomainModel == null)
            {
                return NotFound();
            }

            //map DTO to Domain Model
            RegionDomainModel.Code = updateRegionRequestDTO.Code;
            RegionDomainModel.Name = updateRegionRequestDTO.Name;
            RegionDomainModel.RegionImageURl = updateRegionRequestDTO.RegionImageURl;

            _dbContext.SaveChanges();

            //map domain model back to DTO to client

            var Regiondto = new RegionDto
            {
                Id = RegionDomainModel.Id,
                Name = RegionDomainModel.Name,
                Code = RegionDomainModel.Code,
                RegionImageURl = RegionDomainModel.RegionImageURl
            };

            return Ok(Regiondto);
        }


        //Delete Region
        // Delete : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id : Guid}")]
        public IActionResult Delete([FromRoute] Guid id) 
        {
            var RegionDomainModel = _dbContext.Regions.FirstOrDefault(x=>x.Id == id);
            if (RegionDomainModel == null)
            {
                return NotFound();
            }

            _dbContext.Regions.Remove(RegionDomainModel);
            _dbContext.SaveChanges();

            //optional
            //return deleted region and map domainmodel to DTO
            var Regiondto = new RegionDto
            {
                Id = RegionDomainModel.Id,
                Name = RegionDomainModel.Name,
                Code = RegionDomainModel.Code,
                RegionImageURl = RegionDomainModel.RegionImageURl
            };

            return Ok(Regiondto);   

        }
    }
}
