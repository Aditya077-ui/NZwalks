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
            var walkDTO =  _mapper.Map<List<WalkDTO>>(WalkModel);
            return Ok(walkDTO);
        }
     }
}
