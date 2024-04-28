using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Models.Domain;
using NZwalks.API.Models.DTO;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;
using NZwalks.API.Repositories;
using AutoMapper;
using NZwalks.API.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;

namespace NZwalks.API.Controllers
{
    //https://localhost:portnumber/api/regions   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZwalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        public RegionsController(NZwalksDbContext dbContext, IRegionRepository regionRepository,IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        //GET ALL REGIONS
        //GET : https://localhost:portnumber/api/regions  
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //get data from database - domain models using repository
            var RegionsModel = await _regionRepository.GetAllAsync();
            //When you mark an operation with await,it pauses the execution of the method until the awaited task completes,
            //allowing the calling thread to continue with other work instead of blocking.

            //maps domain models into DTOs

            /*var RegionDTOs = new List<RegionDto>();
            foreach (var RegionModel in RegionsModel)
            {
                RegionDTOs.Add(new RegionDto()
                {
                    Id = RegionModel.Id,
                    Name = RegionModel.Name,
                    Code = RegionModel.Code,
                    RegionImageURl = RegionModel.RegionImageURl,
                });
            }*/
            var RegionDTOs = _mapper.Map<List<RegionDto>>(RegionsModel);

            //return DTOs
            return Ok(RegionDTOs);
        }

        //GET SINGLE REGION BY ID
        // GET : https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //get region domain model from database
            var RegionModel = await _regionRepository.GetByIdAsync(id);
            if (RegionModel == null)
            {
                return NotFound();
            }

            //maps region domain model to Region DTos

           /* var Regiondto = new RegionDto
            {
                Id = RegionModel.Id,
                Name = RegionModel.Name,
                Code = RegionModel.Code,
                RegionImageURl = RegionModel.RegionImageURl,
            };*/
            var Regiondto = _mapper.Map<RegionDto>(RegionModel);
            //return DTO
            return Ok(Regiondto);
        }



        //POST TO CREATE NEW REGION
        //POST:https://localhost:portnumber/api/regions  
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddREgionRequestDTo addREgionRequestDTo)
        {
           //map RegionDTO to domain model
                /* var RegionDomainModel = new Region
                 {
                     Code = addREgionRequestDTo.Code,
                     Name = addREgionRequestDTo.Name,
                     RegionImageURl = addREgionRequestDTo.RegionImageURl
                 };*/
                var RegionDomainModel = _mapper.Map<Region>(addREgionRequestDTo);
                //use domain model to create Region(adding to database)
                await _regionRepository.CreateAsync(RegionDomainModel);


                //map domain model back to DTO
                /* var RegionDTO = new RegionDto
                 {
                     Id = RegionDomainModel.Id,
                     Name = RegionDomainModel.Name,
                     Code = RegionDomainModel.Code,
                     RegionImageURl = RegionDomainModel.RegionImageURl
                 };*/
                var RegionDTO = _mapper.Map<RegionDto>(RegionDomainModel);
                return CreatedAtAction(nameof(GetById), new { id = RegionDTO.Id }, RegionDTO);
            
           
            

        }



        //update Region
        //PUT : https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
           
                //map DTO to domain model
                /*var RegionDomainModel = new Region
                {
                    Code = updateRegionRequestDTO.Code,
                    Name = updateRegionRequestDTO.Name,
                    RegionImageURl = updateRegionRequestDTO.RegionImageURl
                };*/
                var RegionDomainModel = _mapper.Map<Region>(updateRegionRequestDTO);

                //check if Region exists
                RegionDomainModel = await _regionRepository.UpdateAsync(id, RegionDomainModel);
                if (RegionDomainModel == null)
                {
                    return NotFound();
                }

                //THIS PART DONE IN REPOSITORY

                //map DTO to Domain Model
                //RegionDomainModel.Code = updateRegionRequestDTO.Code;
                //RegionDomainModel.Name = updateRegionRequestDTO.Name;
                //RegionDomainModel.RegionImageURl = updateRegionRequestDTO.RegionImageURl;


                //map domain model back to DTO to client

                /* var Regiondto = new RegionDto
                 {
                     Id = RegionDomainModel.Id,
                     Name = RegionDomainModel.Name,
                     Code = RegionDomainModel.Code,
                     RegionImageURl = RegionDomainModel.RegionImageURl
                 }; */
                var Regiondto = _mapper.Map<RegionDto>(RegionDomainModel);

                return Ok(Regiondto);
            
            
        
        }


        //Delete Region
        // Delete : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id) 
        {
            var RegionDomainModel = await _regionRepository.DeleteAsync(id);
            if (RegionDomainModel == null)
            {
                return NotFound();
            }

            //optional
            //return deleted region and map domainmodel to DTO
            var Regiondto = _mapper.Map<RegionDto>(RegionDomainModel);

            return Ok(Regiondto);   

        }
    }
}
