using SchoolRegister.Models;

namespace SchoolRegister.Services.Announcements; 

public interface IAnnouncementsService {
    Task AddAsync(string title, string description, int teacherId);

    Task UpdateAsync(Announcement announcement);

    Task RemoveAsync(int announcementId);

    Task<IEnumerable<Announcement>> GetAllAsync();
}