using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.Data;
using WorldEvents.API.DTOs.Category;
using WorldEvents.API.Helpers;
using WorldEvents.API.Helpers.DataShaper;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly WorldEventsContext _context;
        private readonly IMapper _mapper;
        private readonly IDataShaper<GetCategoryDto> _dataShaper;

        public CategoryService(WorldEventsContext context, IMapper mapper, IDataShaper<GetCategoryDto> dataShaper)
        {
            _context = context;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        public async Task<GetCategoryDto> AddCategory(AddCategoryDto newCategory)
        {
            var category = _mapper.Map<TblCategory>(newCategory);
            try
            {
                await _context.TblCategory.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception();
            }
            

            return _mapper.Map<GetCategoryDto>(category);
        }

        public async Task DeleteCategory(int id)
        {
            var cat = await _context.TblCategory.FirstOrDefaultAsync(x => x.CategoryId == id);
             _context.TblCategory.Remove(cat);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedList<ExpandoObject>> GetCategories(categoryParametres categoryParametres)
        {
            var categories = await _context.TblCategory
                .Skip(categoryParametres.PageSize * (categoryParametres.PageNumber - 1))
                .Take(categoryParametres.PageSize)
                .ApplySort(categoryParametres.orderBy)
                .Select(x => _mapper.Map<GetCategoryDto>(x))
                .ToListAsync();

            var count = await _context.TblCategory.CountAsync();


            var shapedCategory = _dataShaper.ShapeData(categories, categoryParametres.Fields).ToList();
            // return PagedList<ExpandoObject>.ToPagedList(shapedCategory, categoryParametres.PageNumber,categoryParametres.PageSize);
            return new PagedList<ExpandoObject>(shapedCategory, count, categoryParametres.PageNumber, categoryParametres.PageSize);
        }

        public async Task<GetCategoryDto> GetCategoryById(int id)
        {
            var category = await _context.TblCategory.FirstOrDefaultAsync(c => c.CategoryId == id);
            return _mapper.Map<GetCategoryDto>(category);
        }

        public async Task UpdateCategory(UpdateCategoryDto updateCategory)
        {
            var category = await _context.TblCategory.FirstOrDefaultAsync(x => x.CategoryId == updateCategory.CategoryId);
            category.CategoryName = updateCategory.CategoryName;

            _context.TblCategory.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
