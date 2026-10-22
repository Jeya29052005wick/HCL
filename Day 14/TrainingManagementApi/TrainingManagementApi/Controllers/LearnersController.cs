using Microsoft.AspNetCore.Mvc;
using TrainingManagementApi.Models;
using TrainingManagementApi.Services;

namespace TrainingManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearnersController : ControllerBase
{
    private readonly TrainingStore _store;

    public LearnersController(TrainingStore store)
    {
        _store = store;
    }

    [HttpGet]
    public IActionResult GetLearners([FromQuery] string? search)
    {
        IEnumerable<Learner> learners = _store.Learners;

        if (!string.IsNullOrWhiteSpace(search))
        {
            learners = learners.Where(l =>
                l.LearnerId.Contains(search,
                    StringComparison.OrdinalIgnoreCase) ||
                l.Name.Contains(search,
                    StringComparison.OrdinalIgnoreCase) ||
                l.Email.Contains(search,
                    StringComparison.OrdinalIgnoreCase));
        }

        return Ok(learners);
    }

    [HttpPost]
    public IActionResult RegisterLearner(Learner learner)
    {
        if (string.IsNullOrWhiteSpace(learner.LearnerId) ||
            string.IsNullOrWhiteSpace(learner.Name) ||
            string.IsNullOrWhiteSpace(learner.ContactNumber) ||
            string.IsNullOrWhiteSpace(learner.Email))
        {
            return BadRequest(new
            {
                message = "All learner fields are required."
            });
        }

        if (!learner.Email.Contains('@'))
        {
            return BadRequest(new
            {
                message = "Enter a valid email address."
            });
        }

        if (_store.Learners.Any(l =>
            l.LearnerId.Equals(learner.LearnerId.Trim(),
                StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict(new
            {
                message = "That learner ID already exists."
            });
        }

        if (_store.Learners.Any(l =>
            l.Email.Equals(learner.Email.Trim(),
                StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict(new
            {
                message = "That email address is already registered."
            });
        }

        learner.LearnerId = learner.LearnerId.Trim();
        learner.Name = learner.Name.Trim();
        learner.ContactNumber = learner.ContactNumber.Trim();
        learner.Email = learner.Email.Trim();

        _store.Learners.Add(learner);

        return Created($"/api/learners/{learner.LearnerId}", learner);
    }
}