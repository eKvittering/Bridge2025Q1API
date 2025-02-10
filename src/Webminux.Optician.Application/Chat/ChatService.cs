using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Notifications;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webminux.Optician.Authorization.Users;
using Webminux.Optician.Hubs;
using Webminux.Optician.Hubs.Dto;
using Webminux.Optician.Users.Dto;
using static Webminux.Optician.OpticianConsts;

namespace Webminux.Optician.Chat
{
    interface IChatService
    {

    }

    [AbpAuthorize]
    public class ChatService : OpticianAppServiceBase
    {
        private readonly IRepository<Message, int> _repository;
        internal readonly IRepository<User, long> _userRepository;
        internal readonly IObjectMapper _objectMapper;
        private readonly IConfiguration _configuration;
        private readonly INotificationPublisher _notificationPublisher;

        public ChatService(
            IRepository<Message, int> repository,
            IRepository<User, long> userRepository,
            IObjectMapper objectMapper,
            IConfiguration configuration,
            INotificationPublisher notificationPublisher
            )
        {
            _repository = repository;
            _userRepository = userRepository;
            _objectMapper = objectMapper;
            _configuration = configuration;
            _notificationPublisher = notificationPublisher;
        }

        public async Task<List<MessageDto>> GetAllByUser(int userId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant, AbpDataFilters.MustHaveTenant))
            {
                var msgs = await _repository.GetAllListAsync(a => a.UserId == userId || a.CreatorUserId == userId);
                var loggedInUserId = AbpSession.UserId;
                var msgList = new List<MessageDto>();

                foreach (var item in msgs)
                {
                    var m = new MessageDto();
                    m.Date = item.CreationTime;
                    m.ReceiverId = item.UserId;
                    m.SenderId = item.CreatorUserId ?? 0;
                    m.Message = item.Text;
                    m.Type = loggedInUserId == item.CreatorUserId ? MessageType.Sent : MessageType.Received;
                    msgList.Add(m);
                }
                return msgList;

            }
        }
        public async Task SaveMessage(MessageDto msg)
        {
            User defaultAdmin = GetDefaultAdmin();
            if (defaultAdmin == null)
                throw new UserFriendlyException("Problem in finding default admin and tenant");

            long defaultAdminId = defaultAdmin.Id;

            int defaultTenantId = defaultAdmin.TenantId ?? OpticianConsts.DefaultTenantId;

            if (msg.IsAdmin == false)
            {
                msg.ReceiverId = defaultAdminId;
                msg.ReceiverTenantId = defaultTenantId;
            }

            var tenantId = AbpSession.TenantId ?? OpticianConsts.DefaultTenantId;
            try
            {
                var message = new Message();
                message.TenantId = tenantId;
                message.Text = msg.Message;
                message.UserId = (int)msg.ReceiverId;
                message.Type = msg.Type;
                message.CreationTime = DateTime.UtcNow;
                message.CreatorUserId = msg.SenderId;
                await _repository.InsertAsync(message);
                UnitOfWorkManager.Current.SaveChanges();

                var sender = new UserIdentifier(msg.SenderTenantId, msg.SenderId);
                var receiver = new UserIdentifier(msg.ReceiverTenantId, msg.ReceiverId);

                await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(msg.Message),
                severity: NotificationSeverity.Info,
                userIds: new[] { sender, receiver }
            );
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        private User GetDefaultAdmin()
        {
            return _userRepository.GetAll()
                            .OrderBy(u => u.Id).FirstOrDefault();
        }

        /// <summary>
        /// Get Clients with latest messages
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<List<UserDto>> GetCustomerUsers()
        {
            var defaultAdmin = GetDefaultAdmin();
            long defaultAdminId = defaultAdmin.Id;

            var msgs = _repository.GetAll();
            var cutomerUsers = await (from msg in msgs
                                      join user in _userRepository.GetAll() on msg.CreatorUserId equals user.Id
                                      where msg.CreatorUserId != defaultAdminId
                                      select
                                     new UserDto
                                     {
                                         FullName = user.FullName + "(" + msg.TenantId + ")",
                                         Id = user.Id,
                                         CreationTime = msg.CreationTime,
                                         TenantId = user.TenantId ?? DefaultTenantId
                                     })
                                     .OrderByDescending(u => u.CreationTime).ToListAsync();

            var customers = cutomerUsers.GroupBy(x => x.Id).Select(x => new UserDto
            {
                Id = x.First().Id,
                FullName = x.First().FullName,
                TenantId = x.First().TenantId,
            }).ToList();

            return customers;
        }

    }
}
