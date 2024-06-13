using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Social.Domain.Exceptions;
using Social.Domain.Validators.UserProfileValidators;

namespace Social.Domain.Aggregates.UserProfileAggregate
{
    public class BasicInfo
    {
        private BasicInfo()
        {

        }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EmailAddress { get; private set; }
        public string Phone { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string CurrentCity { get; private set; }

        //Factory Method
        public static BasicInfo CreateBasicInfo(string firstName, string lastName, string emailAddress,
            string phone, DateTime dateOfBirth, string currentCity)
        {
            var validator = new BasicInfoValidator();

            var ObjToValidate = new BasicInfo
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Phone = phone,
                DateOfBirth = dateOfBirth,
                CurrentCity = currentCity
            };

            var validationResult = validator.Validate(ObjToValidate);
            if (validationResult.IsValid) return ObjToValidate;

            var exception = new UserProfileNotValidException("The user profile is not valid ");
            foreach (var error in validationResult.Errors)
            {
                exception.ValidationErrors.Add(error.ErrorMessage);
            }

            throw exception;
        }
       
    }
}
