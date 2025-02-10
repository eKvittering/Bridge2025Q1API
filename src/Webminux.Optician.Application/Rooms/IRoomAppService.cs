using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Webminux.Optician.Rooms
{
    /// <summary>
    /// Provide methods to get, add, update and delete rooms
    /// </summary>
    public interface IRoomAppService:IApplicationService
    {
        /// <summary>
        /// Get all rooms
        /// </summary>
        /// <returns>List of rooms</returns>
        Task<ListResultDto<RoomDto>> GetAllAsync();

        /// <summary>
        /// Get room by id
        /// </summary>
        /// <param name="input">Contains Room id</param>
        /// <returns>Room</returns>
        Task<RoomDto> GetAsync(EntityDto<int> input);

        /// <summary>
        /// Add new room
        /// </summary>
        /// <param name="room">Room</param>
        /// <returns>Room</returns>
        Task<RoomDto> AddAsync(CreateRoomDto room);

        /// <summary>
        /// Update room
        /// </summary>
        /// <param name="room">Room</param>
        /// <returns>Room</returns>
        Task<RoomDto> UpdateAsync(RoomDto room);

        /// <summary>
        /// Delete room
        /// </summary>
        /// <param name="input">Contains Room id</param>
        Task Delete(EntityDto input);

        /// <summary>
        /// Get all rooms with pagination
        /// </summary>
        /// <param name="input">Contains page and page size</param>
        /// <returns>List of rooms</returns>
        Task<PagedResultDto<RoomDto>> GetAllPagedAsync(PagedRoomResultRequestDto input);
    }
}
