using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using TrustZoneAPI.DTOs.AccessibilityFeatures;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Places
{
    public interface IPlaceService
    {
        Task<ResponseResult<IEnumerable<PlaceDTO>>> GetAllAsync();
        Task<ResponseResult<PlaceDTO>> GetByIdAsync(int id);
        Task<ResponseResult> AddAsync(CreatePlaceDTO placeDto);
        Task<ResponseResult> UpdateAsync(int id, CreatePlaceDTO placeDto);
        Task<ResponseResult> DeleteAsync(int id);
        Task<ResponseResult<IEnumerable<PlaceDTO>>> GetPlacesByCategoryIdAsync(int categoryId);
        Task<ResponseResult<IEnumerable<PlaceDTO>>> GetPlacesWithFeatureAsync(int featureId);
        Task<ResponseResult<IEnumerable<PlaceDTO>>> FilterPlacesByFeaturesAsync(List<int> featureIds);
        Task<ResponseResult<PlaceWithBranchesDto?>> GetPlaceWithBranchesByIdAsync(int placeId);

        Task<ResponseResult<PlaceBranchDTO>> GetPlaceBranchByIdAsync(int id); // to deal with branch as a place 
        Task<IEnumerable<PlaceSearchDTO>> SearchPlacesAsync(string query, int page, int pageSize);
    }
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IHubContext<SearchHub> _searchHub;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPlaceFeatureService _placeFeatureService;

        public PlaceService(IPlaceRepository placeRepository, IHubContext<SearchHub> searchHub, ICategoryRepository categoryRepository, IPlaceFeatureService placeFeatureService)
        {
            _placeRepository = placeRepository;
            _searchHub = searchHub;
            _categoryRepository = categoryRepository;
            _placeFeatureService = placeFeatureService;
        }

        public async Task<ResponseResult<IEnumerable<PlaceDTO>>> GetAllAsync()
        {
            var places = await _placeRepository.GetAllAsync();

            if (!places.Any())
                return ResponseResult<IEnumerable<PlaceDTO>>.NotFound("No places found");

            var placeDtos = new List<PlaceDTO>();

            foreach (var place in places)
            {
                var dto = _ConvertEntityToDTO(place);

                var featuresResult = await _placeFeatureService.GetFeaturesByPlaceIdAsync(place.Id);
                if (featuresResult.IsSuccess && featuresResult.Data is not null)
                {
                    dto.FeatureIds = featuresResult.Data.Select(f => f.FeatureId).ToList();
                }

                placeDtos.Add(dto);
            }

            return ResponseResult<IEnumerable<PlaceDTO>>.Success(placeDtos);
        }


        public async Task<ResponseResult<PlaceBranchDTO>> GetPlaceBranchByIdAsync(int id)
        {
            var place = await _placeRepository.GetByIdAsync(id);
            if (place == null)
                return ResponseResult<PlaceBranchDTO>.NotFound("Place not found.");
            PlaceBranchDTO dto = _ConvertEntityToDTO(place,"for branch");

            var featuresResult = await _placeFeatureService.GetFeaturesByPlaceIdAsync(id);
            if (featuresResult.IsSuccess && featuresResult.Data is not null)
            {
                dto.Features = featuresResult.Data.ToList();
            }

            return ResponseResult<PlaceBranchDTO>.Success(dto);
        }
        public async Task<ResponseResult<PlaceDTO>> GetByIdAsync(int id)
        {
            var place = await _placeRepository.GetByIdAsync(id);
            if (place == null)
                return ResponseResult<PlaceDTO>.NotFound("Place not found.");
            var dto = _ConvertEntityToDTO(place);

            var featuresResult = await _placeFeatureService.GetFeaturesByPlaceIdAsync(id);
            if (featuresResult.IsSuccess && featuresResult.Data is not null)
            {
                dto.FeatureIds = featuresResult.Data.Select(f => f.FeatureId).ToList();
            }

            return ResponseResult<PlaceDTO>.Success(dto);
        }


        public async Task<ResponseResult> AddAsync(CreatePlaceDTO placeDto)
        {
            if (string.IsNullOrWhiteSpace(placeDto.Name))
                return ResponseResult.Error("Place name is required.", 400);

            var categoryExists = await _categoryRepository.IsCategoryExistsByIdAsync(placeDto.CategoryId);
            if (!categoryExists)
                return ResponseResult.Error("Invalid category ID.", 400);
            var place = _ConvertDTOToEntity(placeDto);

            bool result = await _placeRepository.AddAsync(place);
            if (!result)
                return ResponseResult.Error("Failed to add place", 500);

            if (placeDto.FeatureIds is not null && placeDto.FeatureIds.Any())
            {
                var featureResult = await _placeFeatureService.AddFeatureListToPlaceAsync(new AddFeatureListToPlaceDTO
                {
                    PlaceId = place.Id, 
                    FeatureIds = placeDto.FeatureIds
                });

                if (!featureResult.IsSuccess)
                    return ResponseResult.Error($"Place added but failed to assign some features: {featureResult.ErrorMessage}", featureResult.StatusCode);
            }

            return ResponseResult.Created();
        }


        public async Task<ResponseResult> UpdateAsync(int id, CreatePlaceDTO placeDto)
        {
            if (id <= 0)
                return ResponseResult.Error("Invalid place ID.", 400);

            var existingPlace = await _placeRepository.GetByIdAsync(id);
            if (existingPlace == null)
                return ResponseResult.NotFound("Place not found.");

          _ConvertDTOToEntity(existingPlace, placeDto);
            bool result = await _placeRepository.UpdateAsync(existingPlace);
            return result ? ResponseResult.Success() : ResponseResult.Error("Failed to update place", 500);
        }


        public async Task<ResponseResult> DeleteAsync(int id)
        {
            if (id <= 0)
                return ResponseResult.Error("Invalid place ID.", 400);

            bool result = await _placeRepository.DeleteAsync(id);
            return result ? ResponseResult.Success() : ResponseResult.NotFound("Place not found.");
        }

        public async Task<ResponseResult<IEnumerable<PlaceDTO>>> GetPlacesByCategoryIdAsync(int categoryId)
        {
            var categoryExists = await _categoryRepository.IsCategoryExistsByIdAsync(categoryId);
            if (!categoryExists)
                return ResponseResult<IEnumerable<PlaceDTO>>.Error("Invalid category ID.", 400);
            var places = await _placeRepository.GetPlacesByCategoryIdAsync(categoryId);
            var placeDtos = places.Select(_ConvertEntityToDTO).ToList();
            return ResponseResult<IEnumerable<PlaceDTO>>.Success(placeDtos);
        }

        public async Task<ResponseResult<IEnumerable<PlaceDTO>>> GetPlacesWithFeatureAsync(int featureId)
        {
            var places = await _placeRepository.GetPlacesWithFeatureAsync(featureId);
            var placeDtos = places.Select(_ConvertEntityToDTO).ToList();
            return ResponseResult<IEnumerable<PlaceDTO>>.Success(placeDtos);
        }

        public async Task<IEnumerable<PlaceSearchDTO>> SearchPlacesAsync(string query, int page, int pageSize)
        {
            var places = await _placeRepository.SearchPlacesAsync(query, page, pageSize);

            var results = places.Select(_ConvertEntityToSearchDTO).ToList();
            await _searchHub.Clients.All.SendAsync("ReceiveSearchResults", results);

            return results;
        }

        public async Task<ResponseResult<IEnumerable<PlaceDTO>>> FilterPlacesByFeaturesAsync(List<int> featureIds)
        {
            var places = await _placeRepository.FilterPlacesByFeaturesAsync(featureIds);
            var placeDtos = places.Select(_ConvertEntityToDTO);

            return ResponseResult<IEnumerable<PlaceDTO>>.Success(placeDtos);
        }

       public async  Task<ResponseResult<PlaceWithBranchesDto?>> GetPlaceWithBranchesByIdAsync(int placeId)
        {
            try
            {
                var place = await _placeRepository.GetPlaceWithBranchesByIdAsync(placeId);

                if (place == null)
                {
                    return  ResponseResult<PlaceWithBranchesDto>.NotFound("Place not found.");
                }

                var placeWithBranchesDto = new PlaceWithBranchesDto
                {
                    Id = place.Id,
                    Name = place.Name,
                    Branches = place.Branches.Select(b => new BranchLightDTO
                    {
                        Id = b.Id,
                        Address = b.Address
                    }).ToList()
                };

                return ResponseResult<PlaceWithBranchesDto>.Success(placeWithBranchesDto);
            }
            catch (Exception ex)
            {
                return ResponseResult<PlaceWithBranchesDto>.FromException(ex);
            }
        }



        private PlaceBranchDTO _ConvertEntityToDTO(Place place,string s)
        {
            return new PlaceBranchDTO
            { 
                CategoryId = place.CategoryId,
                Latitude = place.Latitude,
                Longitude = place.Longitude,
                Details = place.Details
            };
        }
        private PlaceDTO _ConvertEntityToDTO(Place place)
        {
           return new PlaceDTO
            {
                Id = place.Id,
                Name = place.Name,
                CategoryId = place.CategoryId,
                Phone = place.Phone,
                Website = place.Website,
                Latitude = place.Latitude,
                Longitude = place.Longitude,
                Details= place.Details
           };
        }

        private  PlaceSearchDTO _ConvertEntityToSearchDTO(Place place)
        {
            return new PlaceSearchDTO
            {
                Id = place.Id,
                Name = place.Name
            };
        }

      
        private Place _ConvertDTOToEntity(CreatePlaceDTO placeDto)
        {
            return new Place
            {
                Name = placeDto.Name,
                CategoryId = placeDto.CategoryId,
                Phone = placeDto.Phone,
                Website = placeDto.Website,
                Latitude = placeDto.Latitude,
                Longitude = placeDto.Longitude,
                Details = placeDto.Details
            };
        }

        private void _ConvertDTOToEntity(Place existingPlace, CreatePlaceDTO placeDto)
        {
             existingPlace.Name = placeDto.Name;
                existingPlace.CategoryId = placeDto.CategoryId;
                existingPlace.Phone = placeDto.Phone;
                existingPlace.Website = placeDto.Website;
                existingPlace.Latitude = placeDto.Latitude;
                existingPlace.Longitude = placeDto.Longitude;
                existingPlace.Details = placeDto.Details;

        }



    }




}
