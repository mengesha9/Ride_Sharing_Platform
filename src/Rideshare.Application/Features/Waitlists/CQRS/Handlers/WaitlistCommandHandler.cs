using AutoMapper;
using MediatR;
using Rideshare.Application.Contracts.Persistence;
using Rideshare.Domain.Entities;
using RideShare.Application.Features.Waitlists.CQRS.Commands;
using RideShare.Application.Features.Waitlists.Dtos.Validators;
using FluentValidation;
using Rideshare.Application.Common.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Rideshare.Application.Common.Exceptions.ValidationException;
using Rideshare.Application.Exceptions;

namespace RideShare.Application.Features.Waitlists.CQRS.Handlers
{
    public class WaitlistCommandHandler : IRequestHandler<WaitlistCommand, Unit>
    {
        private readonly IWaitlistRepository _waitlistRepository;
        private readonly IMapper _mapper;

        public WaitlistCommandHandler(IWaitlistRepository waitlistRepository, IMapper mapper)
        {
            _waitlistRepository = waitlistRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(WaitlistCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate the DTO
            var validator = new WaitlistRequestDtoValidator();
            var validationResult = await validator.ValidateAsync(request.WaitlistRequestDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult); // Return validation errors if validation fails
            }

            // 2. Check if the user is already on the waitlist by email and phone number
            var (emailExists, phoneNumberExists) = await _waitlistRepository.Exists(request.WaitlistRequestDto.Email, request.WaitlistRequestDto.PhoneNumber);

            if (emailExists)
            {
                throw new ConflictException("User is already registered with this email.");
            }

            if (phoneNumberExists)
            {
                throw new ConflictException("User is already registered with this phone number.");
            }

            // 3. Map the DTO to the domain entity
            var waitlistEntity = _mapper.Map<Waitlist>(request.WaitlistRequestDto);

            // 4. Add the entity to the repository
            await _waitlistRepository.Add(waitlistEntity);

            // 5. Return Unit.Value to indicate a successful command execution
            return Unit.Value;
        }
    }
}
