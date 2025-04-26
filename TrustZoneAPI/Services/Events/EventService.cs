using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;
using static TrustZoneAPI.DTOs.Events.EventsDTOs;

namespace TrustZoneAPI.Services.Events
{
    public interface IEventService
    {
        Task<ResponseResult<IEnumerable<AccessibilityEventDto>>> GetAllAsync();
        Task<ResponseResult<AccessibilityEventDto>> GetByIdAsync(int id);
        Task<ResponseResult<AccessibilityEventDto>> CreateAsync(CreateAccessibilityEventDto dto);
        Task<ResponseResult> UpdateAsync(int id, CreateAccessibilityEventDto dto);
        Task<ResponseResult> DeleteAsync(int id);
        Task<ResponseResult<IEnumerable<AccessibilityEventDto>>> GetByBranchIdAsync(int branchId);
        Task<ResponseResult<IEnumerable<AccessibilityEventDto>>> GetByOrganizerIdAsync(string organizerId);
    }
    public class EventService : IEventService
    {

        private readonly IEventRepository _repository;
        private readonly IBranchRepository _branchRepository;

        public EventService(IEventRepository repository, IBranchRepository branchRepository)
        {
            _repository = repository;
            _branchRepository = branchRepository;
        }

        public async Task<ResponseResult<IEnumerable<AccessibilityEventDto>>> GetAllAsync()
        {
            var events = await _repository.GetAllAsync();
            var dtos = new List<AccessibilityEventDto>();

            foreach (var e in events)
            {
                var branch = await _branchRepository.GetByIdAsync(e.BranchId);
                if (branch == null)
                {
                    return ResponseResult<IEnumerable<AccessibilityEventDto>>.Error("Branch not found for one or more events.",401);
                }

                e.Branch = branch;
                dtos.Add(_MapToDto(e));
            }

            return ResponseResult<IEnumerable<AccessibilityEventDto>>.Success(dtos);
        }

        public async Task<ResponseResult<AccessibilityEventDto>> GetByIdAsync(int id)
        {
            var Event  = await _repository.GetByIdAsync(id);
            if (Event ==null)
            {
                return ResponseResult<AccessibilityEventDto>.NotFound("Event not found.");
            }
            var branch = await _branchRepository.GetByIdAsync(Event.BranchId);
            if (branch == null)
            {
                return ResponseResult<AccessibilityEventDto>.NotFound("Branch not found for one or more events.");
            }

                Event.Branch = branch;
            return  ResponseResult<AccessibilityEventDto>.Success(_MapToDto(Event));
        }

        public async Task<ResponseResult<AccessibilityEventDto>> CreateAsync(CreateAccessibilityEventDto dto)
        {
            var newEvent = _MapToEntity(dto);
            var success = await _repository.AddAsync(newEvent);
            if (!success)
                return ResponseResult<AccessibilityEventDto>.Error("Failed to create event.", 500);

            return ResponseResult<AccessibilityEventDto>.Created(_MapToDto(newEvent));
        }

        public async Task<ResponseResult> UpdateAsync(int id, CreateAccessibilityEventDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return ResponseResult.NotFound("Event not found.");

            existing.BranchId = dto.BranchId;
            existing.EventName = dto.EventName;
            existing.Description = dto.Description;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.SpecialFeaturesAvailable = dto.SpecialFeaturesAvailable;

            var success = await _repository.UpdateAsync(existing);
            return success ? ResponseResult.Success() : ResponseResult.Error("Failed to update event.", 500);
        }


        public async Task<ResponseResult> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);
            return success ? ResponseResult.Success() : ResponseResult.NotFound("Event not found.");
        }

        public async Task<ResponseResult<IEnumerable<AccessibilityEventDto>>> GetByBranchIdAsync(int branchId)
        {
            var events = await _repository.GetByBranchIdAsync(branchId);
            var result = events.Select(_MapToDto);
            return ResponseResult<IEnumerable<AccessibilityEventDto>>.Success(result);
        }

        public async Task<ResponseResult<IEnumerable<AccessibilityEventDto>>> GetByOrganizerIdAsync(string organizerId)
        {
            var events = await _repository.GetEventsByOrganizerIdAsync(organizerId);
            var result = events.Select(_MapToDto);
            return ResponseResult<IEnumerable<AccessibilityEventDto>>.Success(result);
        }














        private AccessibilityEvent _MapToEntity(CreateAccessibilityEventDto dto)
        {
            return new AccessibilityEvent
            {
                BranchId = dto.BranchId,
                EventName = dto.EventName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                SpecialFeaturesAvailable = dto.SpecialFeaturesAvailable,
                CreatedAt = DateTime.UtcNow
            };

        }
        private AccessibilityEventDto _MapToDto(AccessibilityEvent e) => new AccessibilityEventDto
        {
            Id = e.Id,
            EventName = e.EventName,
            Description = e.Description,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            SpecialFeaturesAvailable = e.SpecialFeaturesAvailable,
            CreatedAt = e.CreatedAt
        };

    }
}
