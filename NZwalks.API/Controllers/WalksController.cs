using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZwalks.API.Models.Domain;
using NZwalks.API.Models.DTO;
using NZwalks.API.Repositories;

namespace NZwalks.API.Controllers
{
    // api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;

        }
        //Create Walks
        //POST 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //map DTo to domain model
            var WalkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

            //use domain model to create walk
            await _walkRepository.CreateAsync(WalkDomainModel);

            //map domainmodel back to DTO for client
            var walkDTO = _mapper.Map<WalkDTO>(WalkDomainModel);
            return Ok(walkDTO);
        }

        //GET ALl
        //GET 
        [HttpGet]

        public async Task<IActionResult> Getall()
        {
            // get data from database
            var WalkModel = await _walkRepository.GetAllAsync();

            // map domain model to DTO 
            var walkDTO = _mapper.Map<List<WalkDTO>>(WalkModel);
            return Ok(walkDTO);
        }


        // GEt BY ID 
        // GET
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();  
            }

            //map domain model to DTO
            var walkDTO = _mapper.Map<WalkDTO>(walkDomainModel);
            return Ok(walkDTO);
        }



        //update by id 
        //PUT
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDTO updateWalkRequestDTO)
        {
            //map DTO to domain model
            var WalkDomainModel = _mapper.Map<Walk>(updateWalkRequestDTO);

            //use domainmodel to update data in database
            WalkDomainModel =  await _walkRepository.UpdateAsync(id, WalkDomainModel);
            if(WalkDomainModel == null)
            {
                return NotFound();
            }

            //map domainmodel back to DTO
            var walkDTO = _mapper.Map<WalkDTO>(WalkDomainModel);
            return Ok(walkDTO);
        }



        //delete by id
        // DELETE 
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var WalkDomainModel = await _walkRepository.DeleteAsync(id);
            if (WalkDomainModel == null)
            {
                return NotFound();
            }

            //map domain model to DTO 
            var walkdto = _mapper.Map<WalkDTO>(WalkDomainModel);
            return Ok(walkdto);
        }
     }
}
