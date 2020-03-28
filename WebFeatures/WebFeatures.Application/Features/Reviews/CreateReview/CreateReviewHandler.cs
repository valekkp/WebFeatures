﻿using System.Threading;
using System.Threading.Tasks;
using WebFeatures.Application.Infrastructure.Results;
using WebFeatures.Application.Interfaces;
using WebFeatures.Common;
using WebFeatures.Domian.Entities;
using WebFeatures.Requests;

namespace WebFeatures.Application.Features.Reviews.CreateReview
{
    public class CreateReviewHandler : IRequestHandler<CreateReview, Empty>
    {
        private readonly IWebFeaturesDbContext _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _dateTime;

        public CreateReviewHandler(
            IWebFeaturesDbContext db,
            ICurrentUserService currentUser,
            IDateTime dateTime)
        {
            _db = db;
            _currentUser = currentUser;
            _dateTime = dateTime;
        }

        public async Task<Empty> HandleAsync(CreateReview request, CancellationToken cancellationToken)
        {
            var review = new Review(
                _currentUser.UserId,
                request.ProductId,
                request.Title,
                request.Comment,
                _dateTime.Now,
                request.Rating);

            await _db.Reviews.AddAsync(review, cancellationToken);

            return Empty.Value;
        }
    }
}
