using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokemonReviewApp.Tests.Repository;

public class ReviewerRepositoryTests
{
    private readonly IMapper _mapper;
    public ReviewerRepositoryTests()
    {
        _mapper = A.Fake<IMapper>();
    }

    private async Task<DataContext> GetDataContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        DataContext? databaseContext = new DataContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Reviewers.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Add(new Reviewer()
                {
                    Id = i,
                    FirstName = "Mike",
                    LastName = "Smith",
                    Reviews = new List<Review>()
                });
            }
            await databaseContext.SaveChangesAsync();
        }
        return databaseContext;
    }

    [Fact]
    public async void ReviewerRepository_CreateReviewer()
    {
        Reviewer reviewer = A.Fake<Reviewer>();
        var databaseContext = await GetDataContext();
        var repository = new ReviewerRepository(databaseContext, _mapper);

        var result = repository.CreateReviewer(reviewer);

        result.Should().BeTrue();
    }
}
