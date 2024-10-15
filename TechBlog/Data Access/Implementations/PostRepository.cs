﻿using Data_Access.Interfaces;
using Domain_Models;
using DTOs.FilterDto;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.Implementations
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly TechBlogDbContext _context;
        private readonly DbSet<Post> _table;
        public PostRepository(TechBlogDbContext context) : base(context)
        {
            _context = context;
            _table = _context.Set<Post>();
        }

        public Post GetDetailedPost(int id)
        {
            return _table
                .Include(x => x.User)
                .Include(x => x.Stars)
                .Include(x => x.Image)
                .FirstOrDefault(x => x.Id.Equals(id));
        }
        public async Task<PaginatedList> GetPaginatedPosts(int pageIndex, IQueryable<Post> query)
        {
            var pageSize = 12;

            var fullQuery = query
                .Include(x => x.User)
                .Include(x => x.Stars)
                .Include(x => x.Image);

            var posts = await fullQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            return new PaginatedList()
            {
                Posts = posts,
                PageIndex = pageIndex,
                TotalPages = totalPages
            };
        }


    }
}
