﻿using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IBranchOpeningHourRepository : IRepository<BranchOpeningHour,int>
    {
        Task<IEnumerable<BranchOpeningHour>> GetByBranchIdAsync(int branchId);
    }
}
