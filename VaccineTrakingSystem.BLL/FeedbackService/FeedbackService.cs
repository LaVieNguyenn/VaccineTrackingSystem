using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.DAOs.FeedbackDAO;

namespace VaccineTrakingSystem.BLL.FeedbackService
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackDAO _feedbackDAO;

        public FeedbackService(IFeedbackDAO feedbackDAO)
        {
            _feedbackDAO = feedbackDAO;
        }

        private FeedbackDTO MapToDTO(Feedback feedback)
        {
            return new FeedbackDTO
            {
                FeedbackId = feedback.FeedbackId,
                AppointmentId = feedback.AppointmentId,
                CustomerId = feedback.CustomerId,
                Rating = feedback.Rating,
                Comments = feedback.Comments,
                FeedbackDate = feedback.FeedbackDate,
                CreatedAt = feedback.CreatedAt,
                UpdatedAt = feedback.UpdatedAt
            };
        }

        private Feedback MapToEntity(FeedbackDTO dto)
        {
            return new Feedback
            {
                FeedbackId = dto.FeedbackId,
                AppointmentId = dto.AppointmentId,
                CustomerId = dto.CustomerId,
                Rating = dto.Rating,
                Comments = dto.Comments,
                FeedbackDate = dto.FeedbackDate,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

        public async Task<IEnumerable<FeedbackDTO>> GetAllAsync()
        {
            var feedbacks = await _feedbackDAO.GetAllAsync();
            return feedbacks.Select(MapToDTO);
        }

        public async Task<FeedbackDTO?> GetByIdAsync(int id)
        {
            var feedback = await _feedbackDAO.GetByIdAsync(id);
            return feedback != null ? MapToDTO(feedback) : null;
        }

        public async Task<int> CreateAsync(FeedbackDTO feedbackDTO)
        {
            var feedback = MapToEntity(feedbackDTO);
            feedback.CreatedAt = DateTime.UtcNow;
            feedback.UpdatedAt = DateTime.UtcNow;
            feedback.FeedbackDate = DateTime.UtcNow;
            return await _feedbackDAO.InsertAsync(feedback);
        }

        public async Task<bool> UpdateAsync(FeedbackDTO feedbackDTO)
        {
            var existingFeedback = await _feedbackDAO.GetByIdAsync(feedbackDTO.FeedbackId);
            if (existingFeedback == null)
                return false;

            var feedback = MapToEntity(feedbackDTO);
            feedback.UpdatedAt = DateTime.UtcNow;
            feedback.CreatedAt = existingFeedback.CreatedAt;
            feedback.FeedbackDate = existingFeedback.FeedbackDate;
            
            return await _feedbackDAO.UpdateAsync(feedback);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _feedbackDAO.DeleteAsync(id);
        }

        public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksByCustomerId(int customerId, int? appointmentId = null)
        {
            var feedbacks = await _feedbackDAO.GetFeedbacksByCustomerId(customerId, appointmentId);
            return feedbacks.Select(MapToDTO);
        }


        public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksByAppointmentId(int appointmentId)
        {
            var feedbacks = await _feedbackDAO.GetFeedbacksByAppointmentId(appointmentId);
            return feedbacks.Select(MapToDTO);
        }
    }
} 