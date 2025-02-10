using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Services;

public interface ICommentManager : IDomainService
{
    Task CreateAsync(Comment comment);
    IQueryable<Comment> GetAll();
}