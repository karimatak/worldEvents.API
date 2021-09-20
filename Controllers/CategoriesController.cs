using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WorldEvents.API.DTOs.Category;
using WorldEvents.API.Helpers;
using WorldEvents.API.Models;
using WorldEvents.API.Models.ModelsParametres;
using WorldEvents.API.Services.CategoryService;

namespace WorldEvents.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        
        [HttpGet(Name = "GetCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategories([FromQuery]categoryParametres categoryParametres)
        {
            var categories = await _categoryService.GetCategories(categoryParametres);
            if (categories == null)
            {
                return NotFound();
            }
            var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.HasNext,
                categories.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));


            //

            var links = CreateLinksForCategories(categoryParametres,
                categories.HasNext,
                categories.HasPrevious);


            var shapedCategoryWithLinks = categories.Select(category =>
            {
                var categoryAsDictionary = category as IDictionary<string, object>;
               
                 var categoryLinks = CreateLinksForCategory((int)categoryAsDictionary["CategoryId"], null); 

                categoryAsDictionary.Add("links", categoryLinks);
                return categoryAsDictionary;
            });

            var linkedCollectionResource = new
            {
                value = shapedCategoryWithLinks,
                links
            };


            //
            //return Ok(categories);
            return Ok(linkedCollectionResource);
        }


        [HttpGet("{CategoryId}",Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCategoryById(int CategoryId)
        {
            var category = await _categoryService.GetCategoryById(CategoryId);
            if (category == null)
            {
                return NotFound();
            }

            //
            var links = CreateLinksForCategory(category.CategoryId, null);

            //var categoryToReturn = category as IDictionary<string, object>;

            var categoryAsDictionary = ObjectToDictionaryHelper.ToDictionary(category);


            categoryAsDictionary.Add("Links", links);

            //

            //return Ok(category);
            return Ok(categoryAsDictionary);
        }
       
        
        
        [HttpPost(Name = "CreateCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategory(AddCategoryDto newCategory)
        {
            if (newCategory == null)
            {
                return BadRequest();
            }
            var category = await _categoryService.AddCategory(newCategory);

            return CreatedAtRoute(nameof(GetCategoryById), new { Id = category.CategoryId }, category);
        }



        [HttpDelete("{CategoryId}", Name ="DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCategory(int CategoryId)
        {
            var category = await _categoryService.GetCategoryById(CategoryId);

            if (category == null )
            {
                return NotFound();
            }
            await _categoryService.DeleteCategory(CategoryId);
            return NoContent();
        }
        
        
        
        [HttpPut(Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategory)
        {
            var category = await _categoryService.GetCategoryById(updateCategory.CategoryId);
            if (category == null)
            {
                return NotFound();
            }
            await _categoryService.UpdateCategory(updateCategory);
            sayHello();
            return NoContent();
        }


        private void sayHello()
        {
            Console.WriteLine("Hello World");
        }

















        private string CreateCategoryResourceUri(
           categoryParametres categoryParametres,
           ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetCategories",
                        new
                        {
                            fields = categoryParametres.Fields,
                            orderBy = categoryParametres.orderBy,
                            pageNumber = categoryParametres.PageNumber - 1,
                            pageSize = categoryParametres.PageSize
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("GetCategories",
                        new
                        {
                            fields = categoryParametres.Fields,
                            orderBy = categoryParametres.orderBy,
                            pageNumber = categoryParametres.PageNumber + 1,
                            pageSize = categoryParametres.PageSize,

                        });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetCategories",
                        new
                        {
                            fields = categoryParametres.Fields,
                            orderBy = categoryParametres.orderBy,
                            pageNumber = categoryParametres.PageNumber,
                            pageSize = categoryParametres.PageSize,
                        });
            }
        }

        private IEnumerable<Link> CreateLinksForCategory(int categoryId, string fields)
        {
            var links = new List<Link>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new Link(Url.Link("GetCategoryById", new { categoryId }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(
                    new Link(Url.Link("GetCategoryById", new { categoryId, fields }),
                    "self",
                    "GET"));
            }

            links.Add(
                new Link(Url.Link("DeleteCategory", new { categoryId }),
                "delete_category",
                "DELETE"));

            links.Add(
                new Link(Url.Link("CreateCategory", null),
                "create_category",
                "POST"));

            links.Add(
                new Link(Url.Link("UpdateCategory", new { categoryId }),
                "update_category",
                "PUT"));

            return links;
        }

        private IEnumerable<Link> CreateLinksForCategories(
            categoryParametres categoryParametres,
            bool hasNext, bool hasPrevious)
        {
            var links = new List<Link>();

            // self 
            links.Add(
               new Link(CreateCategoryResourceUri(
                   categoryParametres, ResourceUriType.Current)
               , "self", "GET"));

            if (hasNext)
            {
                links.Add(
                  new Link(CreateCategoryResourceUri(
                      categoryParametres, ResourceUriType.NextPage),
                  "nextPage", "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new Link(CreateCategoryResourceUri(
                        categoryParametres, ResourceUriType.PreviousPage),
                    "previousPage", "GET"));
            }

            return links;
        }







    }
}
