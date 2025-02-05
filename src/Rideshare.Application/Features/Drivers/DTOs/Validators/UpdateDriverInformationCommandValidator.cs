using FluentValidation;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Application.Features.Drivers.DTOs.validators
{
    public class UpdateDriverInformationCommandValidator : AbstractValidator<UpdateDriverInformationDto>
    {
        private readonly IDriverRepository _driverRepository;
        public UpdateDriverInformationCommandValidator(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
            RuleFor(driver => driver.FirstName)
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(driver => driver.LastName)
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(driver => driver.Email)
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(driver => driver.Email)
                .MustAsync(async (email, cancellation) => email == null || await _driverRepository.IsEmailUnique(email))
                .WithMessage("Email already exists.");

            RuleFor(driver => driver.Username)
                .MinimumLength(6).WithMessage("Username must be at least 6 characters long.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(driver => driver.Username)
                .MustAsync(async (username, cancellation) => username == null || await _driverRepository.IsUsernameUnique(username))
                .WithMessage("Username already exists.");

            RuleFor(driver => driver.PhoneNumber)
                .Matches(@"^\+[0-9]{1,3}[0-9]{9,15}$")
                .WithMessage("Invalid phone number format.");

            RuleFor(driver => driver.PhoneNumber)
                .MustAsync(async (phoneNumber, cancellation) => phoneNumber == null || await _driverRepository.IsPhoneNumberUnique(phoneNumber))
                .WithMessage("Phone number already exists.");

            RuleFor(driver => driver.LicenseNumber)
                .Matches(@"^[A-Za-z0-9]+$")
                .WithMessage("Invalid license number format.");

            RuleFor(driver => driver.LicenseNumber)
            .MustAsync(async (licenseNumber, cancellation) => licenseNumber == null || await _driverRepository.IsLicenseNumberUnique(licenseNumber)).
            WithMessage("License number already exists.");

            RuleFor(driver => driver.LicenseExpirationDate)
                .Must(BeAValidDate)
                .WithMessage("License expiration date must be a future date.");

            RuleFor(driver => driver.LicensePlateNumber)
                .Matches(@"^[A-Za-z0-9]+$")
                .WithMessage("Invalid license plate number format.")
                .MaximumLength(10).WithMessage("License plate number cannot exceed 10 characters.");

            RuleFor(driver => driver.LicensePlateNumber)
                .MustAsync(async (licensePlateNumber, cancellation) => licensePlateNumber == null || await _driverRepository.IsLicensePlateNumberUnique(licensePlateNumber))
                .WithMessage("License plate number already exists.");

            RuleFor(driver => driver.VehicleType)
                .IsInEnum().WithMessage("Invalid vehicle type.");
        }
        private bool BeAValidDate(DateTime? date)
        {
            if (date == null)
            {
                return true;
            }   
            return date > DateTime.Now;
        }
    }
}
