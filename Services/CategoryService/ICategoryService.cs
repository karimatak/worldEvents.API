using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using WorldEvents.API.DTOs.Category;
using WorldEvents.API.Helpers;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;

namespace WorldEvents.API.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<PagedList<ExpandoObject>> GetCategories(categoryParametres categoryParametres);
        Task<GetCategoryDto> GetCategoryById(int id);
        Task<GetCategoryDto> AddCategory(AddCategoryDto newCategory);
        Task DeleteCategory(int id);
        Task UpdateCategory(UpdateCategoryDto updateCategory);
    }
}
