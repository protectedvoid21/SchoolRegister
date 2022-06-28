using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Announcements; 

public class AnnouncementsService : IAnnouncementsService {
    private readonly SchoolRegisterContext schoolContext;

    public AnnouncementsService(SchoolRegisterContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task AddAsync(string title, string description, int teacherId) {
        Announcement announcement = new() {
            Title = title,
            Description = description,
            TeacherId = teacherId,
        };

        await schoolContext.AddAsync(announcement);
        await schoolContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Announcement announcement) {
        schoolContext.Update(announcement);
        await schoolContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(int announcementId) {
        Announcement announcement = await schoolContext.Announcements.FindAsync(announcementId);
        schoolContext.Remove(announcement);
        await schoolContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Announcement>> GetAllAsync() {
        IEnumerable<Announcement> announcements = await schoolContext.Announcements.ToListAsync();
        return announcements;
    }
}