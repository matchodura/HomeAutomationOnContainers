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
using HomeControl.API.DTOs.LoggingAPI;
using System.Text;
using HomeControl.API.Entities.Enums;

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
        [Route("values")]
        [ProducesResponseType(typeof(RoomDisplayDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<RoomDisplayDTO>> GetRoomWithValues(string roomName)
        {
            if (string.IsNullOrEmpty(roomName)) return BadRequest("Invalid name of the room!");

            var room = await _unitOfWork.RoomRepository.GetRoom(roomName);

            //if (room == null) return NotFound("Room with that name does not exist!");

            //var values = _unitOfWork.RoomValueRepository.GetValue(room.Id);
            //var roomToDisplay = _mapper.Map<RoomDTO>(room);
            //var sensorToDisplay = _mapper.Map<SensorValueDTO>(values);

            //return Ok(new RoomDisplayDTO() { Room = roomToDisplay, Sensor = sensorToDisplay});
            return Ok();
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(List<RoomDisplayDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _unitOfWork.RoomRepository.GetAllRooms();

            if (rooms.Count == 0) return NotFound("There are no configured rooms!");

            return Ok(rooms);
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




        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [Route("layout")]
        public async Task<IActionResult> AddLayout([FromBody] HomeLayoutDTO newLayout)
        {

            //rethink this, currently we are sending the path to the file, works fine locally
            //but about when reading it from remote location(rpi)? fileshare, or pass whole file from
            //the frontend to the controller -> TODO!!

            var file = System.IO.File.ReadAllBytes(newLayout.Layout);

            var layout = _mapper.Map<HomeLayout>(newLayout);

            layout.File = file;

            var currentDate = DateTime.UtcNow;
            layout.LastModified = currentDate;

            _unitOfWork.HomeLayoutRepository.Add(layout);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("layout")]
        public ActionResult GetLayout([FromQuery] int level)
        { 
            var layout =  _unitOfWork.HomeLayoutRepository.Get(level);

            if (layout == null) return NotFound("There is no layout like that!");

            var image =  Convert.ToBase64String(layout.File);            

            return Ok(new {Id = layout.Id, Level = layout.Level, Image = image});
        }

        [HttpGet]
        [Route("names")]
        public ActionResult GetLayoutNames()
        {
            var layouts = _unitOfWork.HomeLayoutRepository.GetAll();

            var test = layouts.Select(x => new LayoutDTO {
                    ID = x.Level,
                    Name = Enum.GetName(typeof(RoomLevel), x.Level)
                 });

            return Ok(test);
        }




    }
}
