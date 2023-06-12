using FakeTimeZone;
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
        private readonly TimeZoneInfo UkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

        private class NoDaylightSavingsAtBoundaryModel<T>
        {
            // Range specified as a floating date/time (no time zone) as recommended for this validator, because the user is not inputing a time zone
            [DateRange("2023-01-01T00:00:00", "2023-12-31T23:59:00")]
            public T? Date { get; set; }

        }

        private class DaylightSavingsAtBoundaryModel<T>
        {
            // Range specified as a floating date/time (no time zone) as recommended for this validator, because the user is not inputing a time zone
            [DateRange("2023-06-01T00:00:00", "2024-05-31T23:59:00")]
            public T? Date { get; set; }
        }

        private class DaylightSavingsAtBoundaryModelWithTimeZone<T>
        {
            // Range specified with a time zone as it's typically returned from a database or DateTime.Now that way
            [DateRange("2023-06-01T00:00:00Z", "2024-05-31T23:59:00Z")]
            public T? Date { get; set; }
        }

        // Values specified as a floating date/time (no time zone) as the user is not able to submit a time zone with the 'Date input' component
        [TestCase("2022-12-31T23:59:00", false)]
        [TestCase("2022-12-31", false)]
        [TestCase("2023-01-01T00:00:00", true)]
        [TestCase("2023-01-01", true)]
        [TestCase("2023-06-01T00:00:00", true)]
        [TestCase("2023-06-01", true)]
        [TestCase("2023-12-31T23:59:00", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-01-01T00:00:00", false)]
        [TestCase("2024-01-01", false)]
        public void Validates_DateTime_property_UK_runtime_environment_Range_boundary_outside_daylight_savings(string date, bool expected)
        {
            using (new FakeLocalTimeZone(UkTimeZone))
            {
                TestDateTime(date, expected);
            }
        }

        // Values specified as a floating date/time (no time zone) as the user is not able to submit a time zone with the 'Date input' component
        [TestCase("2022-12-31T23:59:00", false)]
        [TestCase("2022-12-31", false)]
        [TestCase("2023-01-01T00:00:00", true)]
        [TestCase("2023-01-01", true)]
        [TestCase("2023-06-01T00:00:00", true)]
        [TestCase("2023-06-01", true)]
        [TestCase("2023-12-31T23:59:00", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-01-01T00:00:00", false)]
        [TestCase("2024-01-01", false)]
        public void Validates_DateTime_property_UTC_runtime_environment_Range_boundary_outside_daylight_savings(string date, bool expected)
        {
            using (new FakeLocalTimeZone(TimeZoneInfo.Utc))
            {
                TestDateTime(date, expected);
            }
        }

        // Values specified as a floating date/time (no time zone) as the user is not able to submit a time zone with the 'Date input' component
        [TestCase("2023-05-31T23:59:00", false)]
        [TestCase("2023-05-31", false)]
        [TestCase("2023-06-01T00:00:00", true)]
        [TestCase("2023-06-01", true)]
        [TestCase("2023-12-31T23:59:00", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-05-31T23:59:00", true)]
        [TestCase("2024-05-31", true)]
        [TestCase("2024-06-01T00:00:00", false)]
        [TestCase("2024-06-01", false)]
        public void Validates_DateTime_property_UK_runtime_environment_Range_boundary_within_daylight_savings(string date, bool expected)
        {
            using (new FakeLocalTimeZone(UkTimeZone))
            {
                TestDateTimeDaylightSavings(date, expected);
                TestDateTimeDaylightSavingsWithTimeZone(date, expected);
            }
        }

        // Values specified as a floating date/time (no time zone) as the user is not able to submit a time zone with the 'Date input' component
        [TestCase("2023-05-31T23:59:00", false)]
        [TestCase("2023-05-31", false)]
        [TestCase("2023-06-01T00:00:00", true)]
        [TestCase("2023-06-01", true)]
        [TestCase("2023-12-31T23:59:00", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-05-31T23:59:00", true)]
        [TestCase("2024-05-31", true)]
        [TestCase("2024-06-01T00:00:00", false)]
        [TestCase("2024-06-01", false)]
        public void Validates_DateTime_property_UTC_runtime_environment_Range_boundary_within_daylight_savings(string date, bool expected)
        {
            using (new FakeLocalTimeZone(TimeZoneInfo.Utc))
            {
                TestDateTimeDaylightSavings(date, expected);
                TestDateTimeDaylightSavingsWithTimeZone(date, expected);
            }
        }

        private static void TestDateTime(string date, bool expected)
        {
            // Arrange
            var model = new NoDaylightSavingsAtBoundaryModel<DateTime>
            {
                Date = DateTime.Parse(date)
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.That(results.Count == 0, Is.EqualTo(expected));
        }

        private static void TestDateTimeDaylightSavings(string date, bool expected)
        {
            // Arrange
            var model = new DaylightSavingsAtBoundaryModel<DateTime>
            {
                Date = DateTime.Parse(date)
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.That(results.Count == 0, Is.EqualTo(expected));
        }

        private static void TestDateTimeDaylightSavingsWithTimeZone(string date, bool expected)
        {
            // Arrange
            var model = new DaylightSavingsAtBoundaryModelWithTimeZone<DateTime>
            {
                Date = DateTime.Parse(date)
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.That(results.Count == 0, Is.EqualTo(expected));
        }

        // Values specified as a floating date (no time zone) as the user is not able to submit a time zone with the 'Date input' component
        [TestCase("2022-12-31", false)]
        [TestCase("2023-01-01", true)]
        [TestCase("2023-06-01", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-01-01", false)]
        public void Validates_DateOnly_property_UK_runtime_environment_Range_boundary_outside_daylight_savings(string date, bool expected)
        {
            using (new FakeLocalTimeZone(UkTimeZone))
            {
                TestDateOnly(date, expected);
            }
        }

        // Values specified as a floating date (no time zone) as the user is not able to submit a time zone with the 'Date input' component
        [TestCase("2022-12-31", false)]
        [TestCase("2023-01-01", true)]
        [TestCase("2023-06-01", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-01-01", false)]
        public void Validates_DateOnly_property_UTC_runtime_environment_Range_boundary_outside_daylight_savings(string date, bool expected)
        {
            using (new FakeLocalTimeZone(TimeZoneInfo.Utc))
            {
                TestDateOnly(date, expected);
            }
        }

        // Values specified as a floating date (no time zone) as the user is not able to submit a time zone with the 'Date input' component
        [TestCase("2023-05-31", false)]
        [TestCase("2023-06-01", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-05-31", true)]
        [TestCase("2024-06-01", false)]
        public void Validates_DateOnly_property_UK_runtime_environment_Range_boundary_within_daylight_savings(string date, bool expected)
        {
            using (new FakeLocalTimeZone(UkTimeZone))
            {
                TestDateOnlyDaylightSavings(date, expected);
                TestDateOnlyDaylightSavingsWithTimeZone(date, expected);
            }
        }

        // Values specified as a floating date (no time zone) as the user is not able to submit a time zone with the 'Date input' component
        [TestCase("2023-05-31", false)]
        [TestCase("2023-06-01", true)]
        [TestCase("2023-12-31", true)]
        [TestCase("2024-05-31", true)]
        [TestCase("2024-06-01", false)]
        public void Validates_DateOnly_property_UTC_runtime_environment_Range_boundary_within_daylight_savings(string date, bool expected)
        {
            using (new FakeLocalTimeZone(TimeZoneInfo.Utc))
            {
                TestDateOnlyDaylightSavings(date, expected);
                TestDateOnlyDaylightSavingsWithTimeZone(date, expected);
            }
        }

        private static void TestDateOnly(string date, bool expected)
        {
            // Arrange
            var model = new NoDaylightSavingsAtBoundaryModel<DateOnly>
            {
                Date = DateOnly.Parse(date)
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.That(results.Count == 0, Is.EqualTo(expected));
        }

        private static void TestDateOnlyDaylightSavings(string date, bool expected)
        {
            // Arrange
            var model = new DaylightSavingsAtBoundaryModel<DateOnly>
            {
                Date = DateOnly.Parse(date)
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.That(results.Count == 0, Is.EqualTo(expected));
        }

        private static void TestDateOnlyDaylightSavingsWithTimeZone(string date, bool expected)
        {
            // Arrange
            var model = new DaylightSavingsAtBoundaryModelWithTimeZone<DateOnly>
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
