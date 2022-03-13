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
using System.Net;

namespace HomeControl.API.Controllers
{
    public class RoomController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomController(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{name:string}")]
        [ProducesResponseType(typeof(RoomDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<RoomDTO>> Get(string roomName)
        {
            if (string.IsNullOrEmpty(roomName)) return BadRequest("Invalid name of the room!");

            var room = await _unitOfWork.RoomRepository.GetRoom(roomName);

            if (room == null) return NotFound("Room with that name does not exist!");

            var resultToDisplay = _mapper.Map<RoomDTO>(room);

            return Ok(resultToDisplay);
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(List<RoomDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _unitOfWork.RoomRepository.GetAllRooms();

            if (rooms.Count == 0) return NotFound("There are no configured rooms!");

            var resultsToDisplay = _mapper.Map<RoomDTO>(rooms);

            return Ok(resultsToDisplay);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Create([FromBody] RoomDTO newRoom)
        {
            var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(newRoom.Name);

            if (roomExists == true) return Conflict("Room already exists!");

            var room = _mapper.Map<Room>(newRoom);

            var currentDate = DateTime.UtcNow;
            room.LastModified  = currentDate;

            _unitOfWork.RoomRepository.AddRoom(room);

            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Get), new { roomName = room.Name }, null);

            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Update([FromBody] RoomDTO updatedRoom)
        {
            var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(updatedRoom.Name);

            if (roomExists == false) return NotFound("Room doesn't exist!");

            var room = _mapper.Map<Room>(updatedRoom);

            var currentDate = DateTime.UtcNow;
            room.LastModified = currentDate;

            _unitOfWork.RoomRepository.UpdateRoom(room);

            if (await _unitOfWork.Complete()) return CreatedAtAction(nameof(Get), new { roomName = room.Name }, null);

            return BadRequest();
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(string roomToBeDeleted)
        {
            var roomExists = _unitOfWork.RoomRepository.RoomAlreadyExists(roomToBeDeleted);

            if (roomExists == false) return NotFound("Room doesn't exist!");
                 
            _unitOfWork.RoomRepository.DeleteRoom(roomToBeDeleted);

            if (await _unitOfWork.Complete()) return Ok("Room deleted sucessfuly!");

            return BadRequest();
        }
    }
}
