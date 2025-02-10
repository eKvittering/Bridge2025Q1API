using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Webminux.Optician.Helpers;

namespace Webminux.Optician.Rooms
{
    /// <summary>
    /// Provide methods to get, add, update and delete rooms
    /// </summary>
    public class RoomAppService : OpticianAppServiceBase, IRoomAppService
    {
        private readonly IRepository<Room> _roomRepository;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RoomAppService(IRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Add Room to database
        /// </summary>
        /// <param name="input">Room</param>
        public async Task<RoomDto> AddAsync(CreateRoomDto input)
        {
            var room = Room.Create(GetTenantId(), input.Name, input.Descriptions);
            await _roomRepository.InsertAsync(room);
            return ObjectMapper.Map<RoomDto>(room);
        }


        /// <summary>
        /// Update Room in database
        /// </summary>
        /// <param name="input">Entity Object Contains Room Id</param>
        public async Task Delete(EntityDto input)
        {
            var room = await _roomRepository.GetAsync(input.Id);
            if (room == null)
            {
                throw new EntityNotFoundException("Room not found");
            }

            await _roomRepository.DeleteAsync(room);
        }

        /// <summary>
        /// Get all rooms
        /// </summary>
        /// <returns>List of rooms</returns>
        public async Task<ListResultDto<RoomDto>> GetAllAsync()
        {
            var rooms = await _roomRepository.GetAllListAsync();
            return new ListResultDto<RoomDto>(ObjectMapper.Map<List<RoomDto>>(rooms));
        }

        /// <summary>
        /// Get All Rooms with pagination
        /// </summary>
        /// <param name="input">Contains page and page size</param>
        public async Task<PagedResultDto<RoomDto>> GetAllPagedAsync(PagedRoomResultRequestDto input)
        {
            var query = _roomRepository.GetAll();
            query = ApplyFilters(input, query);
            IQueryable<RoomDto> selectQuery = GetSelectQuery(query);
            var rooms = await selectQuery.GetPagedResultAsync(input.SkipCount, input.MaxResultCount);
            return rooms;
        }

        /// <summary>
        /// Get Room by Id
        /// </summary>
        /// <param name="input">Contains Room Id</param>
        /// <returns>Room</returns>
        public async Task<RoomDto> GetAsync(EntityDto<int> input)
        {
            var room = await _roomRepository.GetAsync(input.Id);
            return ObjectMapper.Map<RoomDto>(room);
        }

        /// <summary>
        /// Update Room in database
        /// </summary>
        /// <param name="input">Room</param>
        /// <returns>Room</returns>
        public async Task<RoomDto> UpdateAsync(RoomDto input)
        {
            var room = await _roomRepository.GetAsync(input.Id);
            if (room == null)
            {
                throw new EntityNotFoundException("Room not found");
            }

            ObjectMapper.Map(input, room);
            return ObjectMapper.Map<RoomDto>(room);
        }

        #region Helpers
        private int GetTenantId()
        {
            return AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
        }

        private static IQueryable<RoomDto> GetSelectQuery(IQueryable<Room> query)
        {
            return query.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Descriptions = r.Descriptions,
                CreationTime = r.CreationTime,
                CreatorUserId = r.CreatorUserId
            });
        }

        private static IQueryable<Room> ApplyFilters(PagedRoomResultRequestDto input, IQueryable<Room> query)
        {
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(x => x.Name.Contains(input.Keyword));
            }

            return query;
        }
        #endregion
    }
}
