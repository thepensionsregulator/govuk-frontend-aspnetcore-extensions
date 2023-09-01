using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    public class TprHostUpdaterTests
    {
        [Test]
        public void Local_is_replaced_by_non_production()
        {
            var updater = new TprHostUpdater();

            var result = updater.UpdateHost("https://example.tpr.local/somepage", "example.nonprod.tpr.gov.uk");

            Assert.AreEqual("https://example.nonprod.tpr.gov.uk/somepage", result);
        }

        [Test]
        public void Local_is_replaced_by_production()
        {
            var updater = new TprHostUpdater();

            var result = updater.UpdateHost("https://example.tpr.local/somepage", "example.thepensionsregulator.gov.uk");

            Assert.AreEqual("https://example.thepensionsregulator.gov.uk/somepage", result);
        }

        [Test]
        public void Non_production_is_replaced_by_local()
        {
            var updater = new TprHostUpdater();

            var result = updater.UpdateHost("https://example.nonprod.tpr.gov.uk/somepage", "example.tpr.local");

            Assert.AreEqual("https://example.tpr.local/somepage", result);
        }

        [Test]
        public void Non_production_is_replaced_by_production()
        {
            var updater = new TprHostUpdater();

            var result = updater.UpdateHost("https://example.nonprod.tpr.gov.uk/somepage", "example.thepensionsregulator.gov.uk");

            Assert.AreEqual("https://example.thepensionsregulator.gov.uk/somepage", result);
        }

        [Test]
        public void Production_is_replaced_by_non_production()
        {
            var updater = new TprHostUpdater();

            var result = updater.UpdateHost("https://example.thepensionsregulator.gov.uk/somepage", "example.nonprod.tpr.gov.uk");

            Assert.AreEqual("https://example.nonprod.tpr.gov.uk/somepage", result);
        }

        [Test]
        public void Production_is_replaced_by_local()
        {
            var updater = new TprHostUpdater();

            var result = updater.UpdateHost("https://example.thepensionsregulator.gov.uk/somepage", "example.tpr.local");

            Assert.AreEqual("https://example.tpr.local/somepage", result);
        }

        [Test]
        public void Hostname_not_matching_pattern_is_ignored()
        {
            var updater = new TprHostUpdater();

            var result = updater.UpdateHost("https://example.org/somepage", "example.tpr.local");

            Assert.AreEqual("https://example.org/somepage", result);
        }

        [Test]
        public void Relative_link_is_ignored()
        {
            var updater = new TprHostUpdater();

            var result = updater.UpdateHost("/somepage", "example.tpr.local");

            Assert.AreEqual("/somepage", result.ToString());
        }
    }
}
