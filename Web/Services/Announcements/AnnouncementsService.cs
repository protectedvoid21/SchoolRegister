﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolRegister.Models;

namespace SchoolRegister.Services.Announcements; 

public class AnnouncementsService : IAnnouncementsService {
    private readonly SchoolContext dbContext;
    private readonly IMapper mapper;

    public AnnouncementsService(SchoolContext dbContext, IMapper mapper) {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task AddAsync(string title, string description, int teacherId) {
        Announcement announcement = new() {
            Title = title,
            Description = description,
            CreateDate = DateTime.Now,
            TeacherId = teacherId,
        };

        await dbContext.AddAsync(announcement);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Announcement announcement) {
        dbContext.Update(announcement);
        await dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(int announcementId) {
        Announcement announcement = await dbContext.Announcements.FindAsync(announcementId);
        dbContext.Remove(announcement);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> IsOwner(int announcementId, int teacherId) {
        Announcement announcement = await dbContext.Announcements
            .FirstAsync(a => a.Id == announcementId);
        return announcement.TeacherId == teacherId;
    }

    public async Task<Announcement> GetById(int id) {
        Announcement announcement = await dbContext.Announcements
            .Include(a => a.Teacher)
            .FirstAsync(a => a.Id == id);
        return announcement;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync<TModel>() {
        IEnumerable<TModel> announcements = dbContext.Announcements
            .ProjectTo<TModel>(mapper.ConfigurationProvider);
        return announcements;
    }

    public async Task<IEnumerable<TModel>> GetAllByTeacher<TModel>(int teacherId) {
        IEnumerable<TModel> announcements = dbContext.Announcements
            .Where(a => a.Teacher.Id == teacherId)
            .ProjectTo<TModel>(mapper.ConfigurationProvider);
        return announcements;
    }
}