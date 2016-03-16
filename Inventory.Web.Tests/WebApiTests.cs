using Nancy;
using Nancy.Testing;
using NUnit.Framework;

namespace Inventory.Web.Tests
{
    [TestFixture]
    public class InventoryWebTests
    {
        private Bootstrapper _bootstrapper;
        private Browser _browser;

        [SetUp]
        public void Setup()
        {
            _bootstrapper = new Inventory.Web.Bootstrapper();
            _browser = new Browser(_bootstrapper);
        }

        [Test]
        public void Should_open_default_page()
        {
            var result = _browser.Get("/", with => { with.HttpRequest(); });
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void Created_item_has_verison_0()
        {
            var response = _browser.Post("/", with =>
            {
                with.HttpRequest(); 
                with.FormValue("name", "dummy_name");
            });

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var model = response.Context.NegotiationContext.DefaultModel;
            Assert.AreEqual(model.Version, 0);
        }

        [Test]
        public void Can_delete_item()
        {
            var responseCreate = _browser.Post("/", with =>
            {
                with.HttpRequest();
                with.FormValue("name", "dummy_name_to_delete");
            });
            Assert.AreEqual(HttpStatusCode.OK, responseCreate.StatusCode);

            var model = responseCreate.Context.NegotiationContext.DefaultModel;
            string url = string.Format("/{0}/{1}", model.Id, model.Version);

            var responseDelete = _browser.Delete(url, with =>
            {
                with.HttpRequest();
            });
            Assert.AreEqual(HttpStatusCode.OK, responseDelete.StatusCode);
        }

        [Test]
        public void Can_add_and_remove_count()
        {
            var responseCreate = _browser.Post("/", with =>
            {
                with.HttpRequest();
                with.FormValue("name", "dummy_name_to_add_and_remove");
            });
            Assert.AreEqual(HttpStatusCode.OK, responseCreate.StatusCode);


            var model = responseCreate.Context.NegotiationContext.DefaultModel;
            string url = string.Format("/Checkin/{0}/{1}", model.Id, model.Version);
            var responseCheckIn = _browser.Post(url, with =>
            {
                with.HttpRequest();
                with.FormValue("number", "123");
            });
            Assert.AreEqual(HttpStatusCode.OK, responseCheckIn.StatusCode);


            model = responseCheckIn.Context.NegotiationContext.DefaultModel;
            url = string.Format("/Checkout/{0}/{1}", model.Id, model.Version);
            var responseCheckOut = _browser.Post(url, with =>
            {
                with.HttpRequest();
                with.FormValue("number", "123");
            });
            Assert.AreEqual(HttpStatusCode.OK, responseCheckOut.StatusCode);
        }

        [Test]
        public void Cannot_remove_more_than_count()
        {
            var responseCreate = _browser.Post("/", with =>
            {
                with.HttpRequest();
                with.FormValue("name", "dummy_name_to_remove");
            });
            Assert.AreEqual(HttpStatusCode.OK, responseCreate.StatusCode);

            // add 1 item
            ///////////////
            var model = responseCreate.Context.NegotiationContext.DefaultModel;
            string url = string.Format("/Checkin/{0}/{1}", model.Id, model.Version);
            var responseCheckIn = _browser.Post(url, with =>
            {
                with.HttpRequest();
                with.FormValue("number", "1");
            });
            Assert.AreEqual(HttpStatusCode.OK, responseCheckIn.StatusCode);

            // remove 2 items => server error!
            ///////////////////////////////////
            model = responseCheckIn.Context.NegotiationContext.DefaultModel;
            url = string.Format("/Checkout/{0}/{1}", model.Id, model.Version);
            var responseCheckOut = _browser.Post(url, with =>
            {
                with.HttpRequest();
                with.FormValue("number", "2");
            });

            // Server Error
            Assert.AreEqual(HttpStatusCode.InternalServerError, responseCheckOut.StatusCode);
        }

        [Test]
        public void Cannot_work_with_deactivated()
        {
            var responseCreate = _browser.Post("/", with =>
            {
                with.HttpRequest();
                with.FormValue("name", "dummy_name_to_delete");
            });
            Assert.AreEqual(HttpStatusCode.OK, responseCreate.StatusCode);

            var model = responseCreate.Context.NegotiationContext.DefaultModel;
            string url = string.Format("/{0}/{1}", model.Id, model.Version);

            var responseDelete = _browser.Delete(url, with => { with.HttpRequest(); });
            Assert.AreEqual(HttpStatusCode.OK, responseDelete.StatusCode);


            // add to 'deactivated' item => server error!
            ////////////////////////////////////////////////
            url = string.Format("/Checkin/{0}/{1}", model.Id, model.Version+1);
            var responseCheckIn = _browser.Post(url, with =>
            {
                with.HttpRequest();
                with.FormValue("number", "666");
            });

            // Server Error
            Assert.AreEqual(HttpStatusCode.InternalServerError, responseCheckIn.StatusCode);
        }
    }
}
