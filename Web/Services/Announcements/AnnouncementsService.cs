using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Announcements; 

public class AnnouncementsService : IAnnouncementsService {
    private readonly SchoolContext schoolContext;

    public AnnouncementsService(SchoolContext schoolContext) {
        this.schoolContext = schoolContext;
    }

    public async Task AddAsync(string title, string description, int teacherId) {
        Announcement announcement = new() {
            Title = title,
            Description = description,
            CreateDate = DateTime.Now,
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

    public async Task<bool> IsOwner(int announcementId, int teacherId) {
        Announcement announcement = await schoolContext.Announcements
            .FirstAsync(a => a.Id == announcementId);
        return announcement.TeacherId == teacherId;
    }

    public async Task<Announcement> GetById(int id) {
        Announcement announcement = await schoolContext.Announcements
            .Include(a => a.Teacher)
            .FirstAsync(a => a.Id == id);
        return announcement;
    }

    public async Task<IEnumerable<Announcement>> GetAllAsync() {
        IEnumerable<Announcement> announcements = await schoolContext.Announcements
            .Include(a => a.Teacher)
            .ToListAsync();
        return announcements;
    }

    public async Task<IEnumerable<Announcement>> GetAllByTeacher(int teacherId) {
        IEnumerable<Announcement> announcements = schoolContext.Announcements.Where(a => a.Teacher.Id == teacherId);
        return announcements;
    }
}