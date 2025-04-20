using TrustZoneAPI.DTOs.Categories;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Categories
{
    public interface ICategoryService
    {
        Task<ResponseResult<IEnumerable<CategoryDTO>>> GetAllAsync();
        Task<ResponseResult<CategoryDTO>> GetByIdAsync(int id);
        Task<ResponseResult> CreateAsync(CreateCategoryDTO dto);
        Task<ResponseResult> UpdateAsync(int id, CreateCategoryDTO dto);
        Task<ResponseResult> DeleteAsync(int id);

        Task<ResponseResult< bool> >IsCategoryExistsByIdAsync(int id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseResult<IEnumerable<CategoryDTO>>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (!categories.Any())
                return ResponseResult<IEnumerable<CategoryDTO>>.NotFound("No categories found");

            var categoryDtos = categories.Select(_ConvertToDTO);
            return ResponseResult<IEnumerable<CategoryDTO>>.Success(categoryDtos);
        }

        public async Task<ResponseResult<CategoryDTO>> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category == null
                ? ResponseResult<CategoryDTO>.NotFound("Category not found.")
                : ResponseResult<CategoryDTO>.Success(_ConvertToDTO(category));
        }

        public async Task<ResponseResult> CreateAsync(CreateCategoryDTO dto)
        {
            string normalizedName = _NormalizeCategoryName(dto.Name);

            if (await _categoryRepository.IsCategoryExistsByNameAsync(normalizedName))
            {
                return ResponseResult.Error("A category with this name already exists.", 400);
            }

            var category = new Category { Name = normalizedName };
            var success = await _categoryRepository.AddAsync(category);
            return success ? ResponseResult.Created() : ResponseResult.Error("Failed to create category.", 500);
        }

        public async Task<ResponseResult> UpdateAsync(int id, CreateCategoryDTO dto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return ResponseResult.NotFound("Category not found.");

            string normalizedName = _NormalizeCategoryName(dto.Name);

            if (await _categoryRepository.IsCategoryExistsByNameAsync(normalizedName) && category.Name != normalizedName)
            {
                return ResponseResult.Error("A category with this name already exists.", 400);
            }

            category.Name = normalizedName;
            var success = await _categoryRepository.UpdateAsync(category);
            return success ? ResponseResult.Success() : ResponseResult.Error("Failed to update category.", 500);
        }

        public async Task<ResponseResult> DeleteAsync(int id)
        {
            var success = await _categoryRepository.DeleteAsync(id);
            return success ? ResponseResult.Success() : ResponseResult.NotFound("Category not found.");
        }


        public async Task<ResponseResult<bool>> IsCategoryExistsByIdAsync(int id)
        {

            var exists = await _categoryRepository.IsCategoryExistsByIdAsync(id);
            return ResponseResult<bool>.Success(exists);
        }

        private static CategoryDTO _ConvertToDTO(Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        private static string _NormalizeCategoryName(string name)
        {
            return name.Trim().ToLower();
        }
    }
}
