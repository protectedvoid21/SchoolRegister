using Data.Models;

namespace Services.Announcements;

public interface IAnnouncementsService {
    Task AddAsync(string title, string description, int teacherId);

    Task UpdateAsync(int id, string title, string description);

    Task RemoveAsync(int id);

    Task<bool> IsOwner(int announcementId, int teacherId);

    Task<Announcement> GetById(int id);

    Task<IEnumerable<TModel>> GetAllAsync<TModel>();

    Task<IEnumerable<TModel>> GetAllByTeacher<TModel>(int teacherId);
}