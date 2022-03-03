using Microsoft.AspNetCore.Mvc;
using HomeControl.API.Controllers.v1;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeControl.API.DTOs;
using HomeControl.API.Interfaces;
using AutoMapper;
using HomeControl.API.Entities;

namespace HomeControl.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }

        [Route("rooms/{id}")]
        [HttpGet]
        public async Task<ActionResult<RoomDTO>> Get(string roomName)
        {
            var room = await _unitOfWork.RoomRepository.GetRoom(roomName);

            if (room == null) return NotFound("Room with that name does not exist!");

            var resultToDisplay = _mapper.Map<RoomDTO>(room);

            return Ok(resultToDisplay);
        }

        //[Route("rooms/all")]
        //[HttpGet]
        //public async Task<ActionResult<List<RoomDTO>>> GetAllRooms()
        //{
        //    var rooms = await _unitOfWork.RoomRepository.GetAllRooms();

        //    if (rooms.Count == 0) return NotFound("There are no configured rooms!");

        //    var resultsToDisplay = _mapper.Map<RoomDTO>(rooms);

        //    return Ok(resultsToDisplay);
        //}

        [Route("rooms")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomDTO newRoom)
        {
            var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(newRoom.Name);

            if (roomExists == true) return Conflict("Room already exists!");

            var room = _mapper.Map<Room>(newRoom);


            _unitOfWork.RoomRepository.AddRoom(room);

            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Get), new { roomName = room.Name }, room);

            return BadRequest();
        }


        [Route("rooms")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoomDTO updatedRoom)
        {
            var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(updatedRoom.Name);

            if (roomExists == false) return NotFound("Room doesn't exist!");

            var room = _mapper.Map<Room>(updatedRoom);

            _unitOfWork.RoomRepository.UpdateRoom(room);

            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Get), new { roomName = room.Name }, room);

            return BadRequest();
        }


        [Route("rooms/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string roomToBeDeleted)
        {
            var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(roomToBeDeleted);

            if (roomExists == false) return Conflict("Room doesn't exist!");
                 
            _unitOfWork.RoomRepository.DeleteRoom(roomToBeDeleted);

            if (await _unitOfWork.Complete()) return Ok("Room deleted sucessfuly!");

            return BadRequest();
        }
    }
}
