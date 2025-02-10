using Abp.Domain.Entities;

namespace Webminux.Optician.Core
{
    public class UserType : Entity, ILookupDto<int>
    {
        public virtual string Name { get; set; }

        protected UserType()
        {
        }

        public static UserType Create(string name)
        {
            return new UserType
            {
                Name = name
            };
        }

    }
}