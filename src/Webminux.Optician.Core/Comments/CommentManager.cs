using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;

public class CommentManager : ICommentManager
{
    private readonly IRepository<Comment> _commentRepository;
    public CommentManager(IRepository<Comment> commentRepository)
    {
        _commentRepository = commentRepository;
    }
    public async Task CreateAsync(Comment comment)
    {
        await _commentRepository.InsertAsync(comment);
    }
    public IQueryable<Comment> GetAll()
    {
        return _commentRepository.GetAll();
    }
}