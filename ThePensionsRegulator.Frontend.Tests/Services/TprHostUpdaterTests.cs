using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.Tests.Services
{
    public class TprHostUpdaterTests
    {
        [Fact]
        public void Local_is_replaced_by_non_production()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("https://example.tpr.local/somepage", "example.nonprod.tpr.gov.uk");

            Assert.Equal("https://example.nonprod.tpr.gov.uk/somepage", result);
        }

        [Fact]
        public void Local_is_replaced_by_production()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("https://example.tpr.local/somepage", "example.thepensionsregulator.gov.uk");

            Assert.Equal("https://example.thepensionsregulator.gov.uk/somepage", result);
        }

        [Fact]
        public void Non_production_is_replaced_by_local()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("https://example.nonprod.tpr.gov.uk/somepage", "example.tpr.local");

            Assert.Equal("https://example.tpr.local/somepage", result);
        }

        [Fact]
        public void Non_production_is_replaced_by_production()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("https://example.nonprod.tpr.gov.uk/somepage", "example.thepensionsregulator.gov.uk");

            Assert.Equal("https://example.thepensionsregulator.gov.uk/somepage", result);
        }

        [Fact]
        public void Production_is_replaced_by_non_production()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("https://example.thepensionsregulator.gov.uk/somepage", "example.nonprod.tpr.gov.uk");

            Assert.Equal("https://example.nonprod.tpr.gov.uk/somepage", result);
        }

        [Fact]
        public void Production_is_replaced_by_local()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("https://example.thepensionsregulator.gov.uk/somepage", "example.tpr.local");

            Assert.Equal("https://example.tpr.local/somepage", result);
        }

        [Fact]
        public void Automatic_enrolment_domain_is_ignored()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("https://example.ae.tpr.gov.uk/somepage", "example.tpr.local");

            Assert.Equal("https://example.ae.tpr.gov.uk/somepage", result);
        }

        [Fact]
        public void Hostname_not_matching_pattern_is_ignored()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("https://example.org/somepage", "example.tpr.local");

            Assert.Equal("https://example.org/somepage", result);
        }

        [Fact]
        public void Relative_link_is_ignored()
        {
            var updater = new TprHostUpdater(Options.Create<TprFrontendOptions>(new()));

            var result = updater.UpdateHost("/somepage", "example.tpr.local");

            Assert.Equal("/somepage", result.ToString());
        }

        [Fact]
        public void Empty_options_array_prevents_replacement()
        {
            var updater = new TprHostUpdater(Options.Create(new TprFrontendOptions { UpdateDestinationHostnames = [] }));

            var result = updater.UpdateHost("https://example.tpr.local/somepage", "example.thepensionsregulator.gov.uk");

            Assert.Equal("https://example.tpr.local/somepage", result);
        }

        [Fact]
        public void Options_array_matching_host_allows_replacement()
        {
            var updater = new TprHostUpdater(Options.Create(new TprFrontendOptions { UpdateDestinationHostnames = ["example"] }));

            var result = updater.UpdateHost("https://example.tpr.local/somepage", "example.thepensionsregulator.gov.uk");

            Assert.Equal("https://example.thepensionsregulator.gov.uk/somepage", result);
        }

        [Fact]
        public void Options_array_not_matching_host_prevents_replacement()
        {
            var updater = new TprHostUpdater(Options.Create(new TprFrontendOptions { UpdateDestinationHostnames = ["differentexample"] }));

            var result = updater.UpdateHost("https://example.tpr.local/somepage", "example.thepensionsregulator.gov.uk");

            Assert.Equal("https://example.tpr.local/somepage", result);
        }
    }
}
