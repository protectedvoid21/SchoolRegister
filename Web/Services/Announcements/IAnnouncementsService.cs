using SchoolRegister.Models;

namespace SchoolRegister.Services.Announcements; 

public interface IAnnouncementsService {
    Task AddAsync(string title, string description, int teacherId);

    Task UpdateAsync(Announcement announcement);

    Task RemoveAsync(int announcementId);

    Task<bool> IsOwner(int announcementId, int teacherId);

    Task<Announcement> GetById(int id);

    Task<IEnumerable<TModel>> GetAllAsync<TModel>();

    Task<IEnumerable<TModel>> GetAllByTeacher<TModel>(int teacherId);
}