using Inventory.Dtos;

namespace Inventory.Clients
{
    public class EmployeeClient
    {
        private readonly HttpClient httpClient;

        public EmployeeClient(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<StaffDto> GetStaffInformationAsync(Guid staffId)
        {
            var staff = await httpClient.GetFromJsonAsync<StaffDto>("/api/StaffGeneric/" + staffId);
            return staff;
        }

        public async Task<IReadOnlyCollection<StaffDto>> GetAllStaff()
        {
            var staffList = await httpClient.GetFromJsonAsync<IReadOnlyCollection<StaffDto>>("/api/StaffGeneric");
            return staffList;
        }
    }
}
