using BlogProjem.Data.Abstract;
using BlogProjem.Services.Abstract;
using BlogProjem.Shared.Utilities.Results.Abstract;
using BlogProjem.Shared.Utilities.Results.ComplexTypes;
using BlogProjem.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Services.Concrete
{
    public class CommentManager : ICommentService
    {

        private readonly IUnitOfWork _unitOfWork;

        public async Task<IDataResult<int>> Count()
        {
            var commentsCount = await _unitOfWork.Comments.CountAsync();
            if (commentsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı", -1);
            }
        }

        public async Task<IDataResult<int>> CountByIsDeleted()
        {
            var commentsCount = await _unitOfWork.Comments.CountAsync(c => !c.IsDeleted);
            if (commentsCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı", -1);
            }
        }
    }
}
