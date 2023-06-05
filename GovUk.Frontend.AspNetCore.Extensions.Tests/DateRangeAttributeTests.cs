using GovUk.Frontend.AspNetCore.Extensions.Validation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    [TestFixture]
    public class DateRangeAttributeTests
    {
        private class ExampleModel<T>
        {
            [DateRange("2023-01-01T00:00:00Z", "2023-12-31T23:59:00Z")]
            public T? Date { get; set; }
        }

        [TestCase("2022-12-31T23:59:00Z", false)]
        [TestCase("2023-01-01T00:00:00Z", true)]
        [TestCase("2023-12-31T23:59:00Z", true)]
        [TestCase("2024-01-01T00:00:00Z", false)]
        public void Validates_DateTime_property(string date, bool expected)
        {
            // Arrange
            var model = new ExampleModel<DateTime>
            {
                Date = DateTime.Parse(date)
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.That(results.Count == 0, Is.EqualTo(expected));
        }

        [TestCase("2022-12-31", false)]
        [TestCase("2023-01-01", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-01-01", false)]
        public void Validates_DateOnly_property(string date, bool expected)
        {
            // Arrange
            var model = new ExampleModel<DateOnly>
            {
                Date = DateOnly.Parse(date)
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.That(results.Count == 0, Is.EqualTo(expected));
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
